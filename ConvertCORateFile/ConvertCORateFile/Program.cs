﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ConvertCORateFile
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>args[0] = File path and name</remarks>
        /// <remarks>args[1] = url for Tax Form Catalog</remarks>
        /// <remarks>args[2] = user id for Tax Form Catalog</remarks>
        /// <remarks>args[3] = password for Tax Form Catalog</remarks>
        static void Main(string[] args)
        {
            List<InputData> filingSystems = new List<InputData>(); // = null;

            //Populate object with source file
            using (FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {

                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fs, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    Sheet mysheet = (Sheet) workbookPart.Workbook.Sheets.ChildElements.GetItem(1);

                    foreach (Row row in mysheet.Elements<Row>())
                    {
                        InputData filingSystem = new InputData();
                        int rowcolumn = 1;

                        foreach (Cell c in row.Elements<Cell>())
                        {

                            switch (rowcolumn)
                            {
                                case 1: filingSystem.TaxFormCode = ReadExcelCell(c, workbookPart); break;
                                case 2: filingSystem.State = ReadExcelCell(c, workbookPart); break;
                                case 3: filingSystem.FilingSystem = ReadExcelCell(c, workbookPart) ?? null; break;
                            }

                            rowcolumn++;
                        }

                        filingSystems.Add(filingSystem);
                    }
                }
            }

            int totalRecords = filingSystems.Count;
            Int64 formMasterId = 0;
            int recordsInserted = 0;
            int recordsErrored = 0;

            // Create List of FormCellDependency records to add 
            foreach (InputData item in filingSystems)
            {
                formMasterId = GetFormMasterId(item.TaxFormCode, args[1], args[2], args[3]);
                if (formMasterId <= 0)
                {
                    Console.WriteLine(string.Format("1: Unable to find a formmaster record for taxformcode [{0}].", item.TaxFormCode));
                }
                else
                {
                    try
                    {
                        Int64 filingSystemId = 0;
                        switch (item.FilingSystem.ToLower())
                        {
                            case "transmission":
                                filingSystemId = 1;
                                break;
                            case "skyscraper":
                                filingSystemId = 2;
                                break;
                            case "onetransmission":
                                filingSystemId = 3;
                                break;
                            default:
                                filingSystemId = 0;
                                break;
                        }    
                        List<FormFilingSystem> existingFilingSystems = GetFormFilingSystems(formMasterId, args[1], args[2], args[3]);
                        FormFilingSystem formFilingSystem = existingFilingSystems.Any() ? existingFilingSystems.Where<FormFilingSystem>(x => x.FilingSystemId == filingSystemId).FirstOrDefault() : null;
                        if (formFilingSystem == null || formFilingSystem == new FormFilingSystem() )
                        { 
                            FormFilingSystem filingSystemToAdd = new FormFilingSystem();
                            filingSystemToAdd.FormFilingSystemId = -1;
                            filingSystemToAdd.FormMasterId = formMasterId;
                            filingSystemToAdd.FilingSystemId = filingSystemId;
                            filingSystemToAdd.Disabled = false;
                            filingSystemToAdd.IsDefault = existingFilingSystems.Count > 0 ? false : true;
                            filingSystemToAdd.GenerateTransmissionFile = filingSystemId == 1 || filingSystemId == 3 ? true : false;

                            Int64 insertedFilingSystemId = -1;
                            insertedFilingSystemId = InsertFormFilingSystem(filingSystemToAdd, args[1], args[2], args[3]);
                            if (insertedFilingSystemId > 0)
                            {
                                recordsInserted++;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("2: Insert to FormFilingSystem did not return a valid FormFilingSystemId for TaxFormCode [{0}].", item.TaxFormCode));
                                recordsErrored++;
                            }

                        }
                        else
                        {
                            Console.WriteLine(string.Format("3: An Existing FormFilingSystemRecord exists for TaxFormCode [{0}] and Filing System [{1}].", item.TaxFormCode, item.FilingSystem));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("4: LoadFormFilingSystem: An error occurred [{0}] while processing TaxFormCode [{1}] and filing system [{2}].", ex.Message, item.TaxFormCode, item.FilingSystem));
                        recordsErrored++;
                    }

                }
            }

            Console.WriteLine(string.Format("5: LoadFormFilingSystem: {0} records inserted and {1} records errored out of {2} total records.", recordsInserted.ToString(), recordsErrored.ToString(), totalRecords.ToString()));
            Console.ReadKey();
        }
 
        private static string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        /// <summary>
        /// obtains the list of FormFilingSystems for a given FormMaster
        /// </summary>
        /// <param name="FormMasterId"></param>
        /// <param name="Url"></param>
        /// <param name="Username"></param>
        /// <param name="Drowssap"></param>
        /// <returns></returns>
        private static List<FormFilingSystem> GetFormFilingSystems(long FormMasterId, string Url, string Username, string Drowssap)
        {
            List<FormFilingSystem> results = new List<FormFilingSystem>();

            try
            {
                string query = string.Format("/api/FormFilingSystem?FormMasterId={0}", FormMasterId.ToString());

                // Create the http web request.
                var request = (HttpWebRequest)WebRequest.Create(Url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                // execute the request.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<FormFilingSystem>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("6: GetFormFilingSystem: An unhandled exception occurred:[{0}]", ex.Message);

                return results;
            }

            return results;
        }

        /// <summary>
        /// obtains the formmaster for a given TaxFormCode
        /// </summary>
        /// <param name="TaxFormCode"></param>
        /// <param name="Url"></param>
        /// <param name="Username"></param>
        /// <param name="Drowssap"></param>
        /// <returns></returns>
        private static long GetFormMasterId(string TaxFormCode, string Url, string Username, string Drowssap)
        {
            Int64 formMasterId = -1;
            FormMaster formMaster;
            string query = string.Empty;

            try
            {
                query = string.Format("/api/FormMaster/%7Bid%7D?taxFormCode={0}", TaxFormCode);

                // Create the http web request.
                var request1 = (HttpWebRequest)WebRequest.Create(Url + query);
                request1.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                // execute the request.
                using (var response1 = (HttpWebResponse)request1.GetResponse())
                {
                    if (response1 != null)
                    {
                        var responseStream1 = response1.GetResponseStream();
                        var streamReader1 = new System.IO.StreamReader(responseStream1, System.Text.Encoding.UTF8);
                        var responseString1 = streamReader1.ReadToEnd();

                        formMaster = responseString1.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<FormMaster>(responseString1) : null;
                        if (formMaster != null)
                        {
                            formMasterId = formMaster.FormMasterId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("7: GetFormMasterId 1: Unable to find TaxFormCode [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);
            }


            try
            {
                if (formMasterId == -1)
                {
                    query = string.Format("/api/FormMaster/%7Bid%7D?legacyReturnName={0}", TaxFormCode);
                    var request2 = (HttpWebRequest)WebRequest.Create(Url + query);
                    request2.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                    using (var response2 = (HttpWebResponse)request2.GetResponse())
                    {
                        if (response2 != null)
                        {
                            var responseStream2 = response2.GetResponseStream();
                            var streamReader2 = new System.IO.StreamReader(responseStream2, System.Text.Encoding.UTF8);
                            var responseString2 = streamReader2.ReadToEnd();

                            formMaster = responseString2.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<FormMaster>(responseString2) : null;
                            if (formMaster != null)
                            {
                                formMasterId = formMaster.FormMasterId;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("8: GetFormMasterId 2: Unable to find LegacyReturnName [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);

                return formMasterId;
            }

            return formMasterId;
        }

        private static Int64 InsertFormFilingSystem(FormFilingSystem filingSystem, string url, string Username, string Drowssap)
        {
            string payload = string.Empty;
            Int64 formFilingSystemId = -1;
            try
            {
                // get post data.
                payload = JsonConvert.SerializeObject(filingSystem);
                byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/FormFilingSystem";
                var request = (HttpWebRequest)WebRequest.Create(url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                request.Method = "POST";
                request.ContentLength = buf.Length;
                request.ContentType = "application/json; charset=utf-8";
                request.GetRequestStream().Write(buf, 0, buf.Length);

                // get response and deserialize it.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        FormFilingSystem responseDependency = responseString.Length > 0 ? JsonConvert.DeserializeObject<FormFilingSystem>(responseString) : null;
                        formFilingSystemId = responseDependency.FormFilingSystemId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("9: InsertFormFilingSystem: The following attempted insert/update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return formFilingSystemId;
        }

    }

    private static void ExportList(List<ReportData> dueDates, string destination)
    {
        using (SpreadsheetDocument document = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Due Dates" };

            sheets.Append(sheet);

            workbookPart.Workbook.Save();

            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            // Create Header Row
            DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
            headerRow.Append(
                new Cell()
                {
                    CellValue = new CellValue("FormMasterId"),
                    DataType = new EnumValue<CellValues>(CellValues.Number)
                },
                new Cell()
                {
                    CellValue = new CellValue("Country"),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                },
                new Cell()
                {
                    CellValue = new CellValue("Region"),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                },
                new Cell()
                {
                    CellValue = new CellValue("TaxFormCode"),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                },
                new Cell()
                {
                    CellValue = new CellValue("Description"),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                },
                new Cell()
                {
                    CellValue = new CellValue("Domains"),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                },
                new Cell()
                {
                    CellValue = new CellValue("DueDate"),
                    DataType = new EnumValue<CellValues>(CellValues.Date)
                }
            );

            sheetData.AppendChild(headerRow);

            // Add Data Rows to SheetData
            foreach (ReportData rd in dueDates)
            {
                DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                newRow.Append(
                    new Cell()
                    {
                        CellValue = new CellValue(rd.FormMasterId.ToString()),
                        DataType = new EnumValue<CellValues>(CellValues.Number)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.Country),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.Region),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.TaxFormCode),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.Description),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.Domains),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    },
                    new Cell()
                    {
                        CellValue = new CellValue(rd.DueDate),
                        DataType = new EnumValue<CellValues>(CellValues.Date)
                    }
                );
                sheetData.AppendChild(newRow);
            }
            worksheetPart.Worksheet.Save();
        }
    }
}

public class ReportData
{
    public Int64 FormMasterId { get; set; }
    public String Country { get; set; }
    public String Region { get; set; }
    public String TaxFormCode { get; set; }
    public String Description { get; set; }
    public String Domains { get; set; }
    public DateTime DueDate { get; set; }
}

public class FormDueDate
{
    public Int64 FormDueDateId { get; set; }
    public Int64 FormMasterId { get; set; }
    public Int64 FilingMethodId { get; set; }
    public Int64 FilingFrequencyId { get; set; }
    public DateTime DueDate { get; set; }
    public Int64 CreatedUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Int64 ModifiedUserId { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Int64 PaymentMethodId { get; set; }
}


public class InputData
    {
        public String TaxFormCode{ get; set; }
        public String State { get; set; }
        public String FilingSystem { get; set; }
    }

    public class FormFilingSystem
    {
        public Int64 FormFilingSystemId { get; set; }
        public Int64 FormMasterId { get; set; }
        public Int64 FilingSystemId { get; set; }
        public Boolean Disabled { get; set; }
        public DateTime? ExpectedAvailabilityDate { get; set; }
        public String DisabledReason { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean IsDefault { get; set; }
        public Boolean GenerateTransmissionFile { get; set; }
        public String JsonBlob { get; set; }
    }


    public partial class FormMaster
    {
        public Int64 FormMasterId { get; set; }
        public Int64 FormTypeId { get; set; }
        public String TaxFormCode { get; set; }
        public String LegacyReturnName { get; set; }
        public String TaxFormName { get; set; }
        public String Description { get; set; }
        public Boolean IsEffective { get; set; }
        public String Country { get; set; }
        public String Region { get; set; }
        public String AuthorityName { get; set; }
        public String ShortCode { get; set; }
        public Int16? DueDay { get; set; }
        public Int16? DelinquentDay { get; set; }
        public Byte? FiscalYearStartMonth { get; set; }
        public Boolean HasMultiFrequencies { get; set; }
        public Boolean IsPOARequired { get; set; }
        public Boolean IsRegistrationRequired { get; set; }
        public Boolean HasMultiRegistrationMethods { get; set; }
        public Boolean HasSchedules { get; set; }
        public Boolean HasMultiFilingMethods { get; set; }
        public Boolean HasMultiPayMethods { get; set; }
        public Boolean IsEFTRequired { get; set; }
        public Boolean IsFilePayMethodLinked { get; set; }
        public Int64 MailingReceivedRuleId { get; set; }
        public Int64 ProofOfMailingId { get; set; }
        public Boolean IsNegAmountAllowed { get; set; }
        public Boolean AllowNegativeOverallTax { get; set; }
        public Boolean IsNettingRequired { get; set; }
        public Int64 RoundingMethodId { get; set; }
        public Decimal VendorDiscountAnnualMax { get; set; }
        public Boolean VersionsRequireAuthorityApproval { get; set; }
        public Int64 OutletReportingMethodId { get; set; }
        public Boolean HasReportingCodes { get; set; }
        public Boolean HasPrepayments { get; set; }
        public Boolean GrossIncludesInterstateSales { get; set; }
        public String GrossIncludesTax { get; set; }
        public Boolean HasEfileFee { get; set; }
        public Boolean HasEpayFee { get; set; }
        public Boolean HasDependencies { get; set; }
        public String RequiredEfileTrigger { get; set; }
        public String RequiredEftTrigger { get; set; }
        public Boolean VendorDiscountEfile { get; set; }
        public Boolean VendorDiscountPaper { get; set; }
        public Boolean? PeerReviewed { get; set; }
        public Int64? PeerReviewedId { get; set; }
        public DateTime? PeerReviewedDate { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public String DORAddressMailTo { get; set; }
        public String DORAddress1 { get; set; }
        public String DORAddress2 { get; set; }
        public String DORAddressCity { get; set; }
        public String DORAddressRegion { get; set; }
        public String DORAddressPostalCode { get; set; }
        public String DORAddressCountry { get; set; }
        public String ZeroAddressMailTo { get; set; }
        public String ZeroAddress1 { get; set; }
        public String ZeroAddress2 { get; set; }
        public String ZeroAddressCity { get; set; }
        public String ZeroAddressRegion { get; set; }
        public String ZeroAddressPostalCode { get; set; }
        public String ZeroAddressCountry { get; set; }
        public String AmendedAddressMailTo { get; set; }
        public String AmendedAddress1 { get; set; }
        public String AmendedAddress2 { get; set; }
        public String AmendedAddressCity { get; set; }
        public String AmendedAddressRegion { get; set; }
        public String AmendedAddressPostalCode { get; set; }
        public String AmendedAddressCountry { get; set; }
        public Boolean OnlineBackFiling { get; set; }
        public Boolean OnlineAmendedReturns { get; set; }
        public String PrepaymentFrequency { get; set; }
        public Boolean? OutletLocationIdentifiersRequired { get; set; }
        public String ListingSortOrder { get; set; }
        public String DORWebsite { get; set; }
        public Boolean FileForAllOutlets { get; set; }
        public Boolean PaperFormsDoNotHaveDiscounts { get; set; }
        public Boolean StackAggregation { get; set; }
        public Byte? RoundingPrecision { get; set; }
        public Decimal? InconsistencyTolerance { get; set; }
        public DateTime EffDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean VisibleToCustomers { get; set; }
        public Boolean RequiresOutletSetup { get; set; }
        public Boolean AchCreditAllowed { get; set; }
        public String ReportLevel { get; set; }
        public Boolean PostOfficeValidated { get; set; }
        public String StackAggregationOption { get; set; }
        public String SstBehavior { get; set; }
        public String NonSstBehavior { get; set; }
        public String DORPhoneNumber { get; set; }
        public Int64? AverageCheckClearDays { get; set; }
        public Boolean FilterZeroRatedLineDetails { get; set; }
        public Boolean AllowsBulkFilingAccounts { get; set; }
        public String BulkAccountInstructionLink { get; set; }
        public String RegistrationIdFormat { get; set; }
        public String ThresholdTrigger { get; set; }
        public String TransactionSortingOption { get; set; }
        public Int64? ContentReviewFrequencyId { get; set; }
        public Int64? AliasForFormMasterId { get; set; }
        public Int32 DefaultPrepayPercentage { get; set; }
        public Boolean AllowFixedPrepayAmount { get; set; }
        public Int64 BatchedEnvelopeRuleId { get; set; }
        public Int64? FormLogicTypeId { get; set; }
    }

}
