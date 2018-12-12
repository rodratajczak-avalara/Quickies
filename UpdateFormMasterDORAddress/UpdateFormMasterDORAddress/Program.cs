using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;

namespace UpdateFormMasterDORAddress
{
    class Program
    {
        static void Main(string[] args)
        {
            List<InputData> dorAddresses = new List<InputData>();

            //Populate object with source file
            using (FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {

                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fs, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        InputData dorAddress = new InputData();
                        int rowcolumn = 1;
                        List<String> firstRow = new List<String>();

                        foreach (Cell c in row.Elements<Cell>())
                        {

                            switch (rowcolumn)
                            {
                                case 1: dorAddress.TaxFormCode = ReadExcelCell(c, workbookPart); break;
                                case 2: dorAddress.LegacyReturnName = ReadExcelCell(c, workbookPart); break;
                                case 3: dorAddress.DORAddressMailto = ReadExcelCell(c, workbookPart); break;
                                case 4: dorAddress.DORAddress1 = ReadExcelCell(c, workbookPart); break;
                                case 5: dorAddress.DORAddress2 = ReadExcelCell(c, workbookPart); break;
                                case 6: dorAddress.DORAddressCity = ReadExcelCell(c, workbookPart); break;
                                case 7: dorAddress.DORAddressRegion = ReadExcelCell(c, workbookPart); break;
                                case 8: dorAddress.DORAddressPostalCode = ReadExcelCell(c, workbookPart); break;
                                case 9: dorAddress.Comments = ReadExcelCell(c, workbookPart); break;
                            }

                            rowcolumn++;
                        }

                        dorAddresses.Add(dorAddress);
                    }
                }
            }

            int totalRecords = dorAddresses.Count;
            int recordsUpdated = 0;
            int recordsErrored = 0;
            int recordsSkipped = 0;

            // Create List of FormCellDependency records to add 
            foreach (InputData item in dorAddresses)
            {
                FormMaster currentFormMaster = GetFormMaster(item.TaxFormCode, args[1], args[2], args[3]);
                if (currentFormMaster == null)
                {
                    Console.WriteLine(string.Format("1: Unable to find a formmaster record for taxformcode [{0}].", item.TaxFormCode));
                }
                else
                {
                    try
                    {
                        if (currentFormMaster.ModifiedDate < DateTime.Parse("2018-11-01"))
                        {
                            currentFormMaster.DORAddressMailTo = item.DORAddressMailto;
                            currentFormMaster.DORAddress1 = item.DORAddress1;
                            currentFormMaster.DORAddress2 = item.DORAddress2;
                            currentFormMaster.DORAddressCity = item.DORAddressCity;
                            currentFormMaster.DORAddressRegion = item.DORAddressRegion;
                            currentFormMaster.DORAddressPostalCode = item.DORAddressPostalCode;
                            currentFormMaster.ModifiedDate = System.DateTime.UtcNow;
                            currentFormMaster.ModifiedUserId = 234
                            bool success = UpdateFormMasterDORAddress(currentFormMaster, args[1], args[2], args[3]);
                            if (success)
                            {
                                Console.WriteLine(string.Format("UpdateFormMasterAddress: TaxFormCode [{0}] was successfully updated.", item.TaxFormCode));
                                recordsUpdated++;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("UpdateFormMasterAddress: TaxFormCode [{0}] was not updated.", item.TaxFormCode));
                                recordsErrored++;
                            }
                        }
                        else
                        {
                            Console.WriteLine(string.Format("UpdateFormMasterAddress: TaxFormCode [{0}] was not updated because it was modified after 2018-11-01.", item.TaxFormCode));
                            recordsSkipped++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("UpdateFormMasterAddress: An error occurred [{0}] while processing TaxFormCode [{1}].", ex.Message, item.TaxFormCode));
                        recordsErrored++;
                    }

                }
            }

            Console.WriteLine(string.Format("UpdateFormMasterDORAddresses: {0} records updated out of {1} total records.  There were {2} records that resulted in an error. There were {3} records that were skipped because they had been modified after 2018-11-01.", recordsUpdated.ToString(), totalRecords.ToString(), recordsErrored.ToString(), recordsSkipped.ToString()));
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
        /// obtains the formmaster for a given TaxFormCode
        /// </summary>
        /// <param name="TaxFormCode"></param>
        /// <param name="Url"></param>
        /// <param name="Username"></param>
        /// <param name="Drowssap"></param>
        /// <returns></returns>
        private static FormMaster GetFormMaster(string TaxFormCode, string Url, string Username, string Drowssap)
        {
            FormMaster formMaster = null;
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("7: GetFormMaster 1: Unable to find TaxFormCode [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);
            }


            try
            {
                if (formMaster == null)
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("8: GetFormMaster 2: Unable to find LegacyReturnName [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);
            }

            return formMaster;
        }

        private static Boolean UpdateFormMasterDORAddress(FormMaster formMaster, string url, string Username, string Drowssap)
        {
            string payload = string.Empty;
            Boolean success = false;
            try
            {
                // get post data.
                payload = JsonConvert.SerializeObject(formMaster);
                byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/FormMaster";
                var request = (HttpWebRequest)WebRequest.Create(url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                request.Method = "PUT";
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

                        FormMaster responseFormMaster = responseString.Length > 0 ? JsonConvert.DeserializeObject<FormMaster>(responseString) : null;
                        if (responseFormMaster != null)
                        {
                            success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("9: UpdateFormMasterDORAddress: The following attempted update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return success;
        }

    }


    public class InputData
    {
        public String TaxFormCode { get; set; }
        public String LegacyReturnName { get; set; }
        public String DORAddressMailto { get; set; }
        public String DORAddress1 { get; set; }
        public String DORAddress2 { get; set; }
        public String DORAddressCity { get; set; }
        public String DORAddressRegion { set; get; }
        public String DORAddressPostalCode { get; set; }
        public String Comments { get; set; }
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
        public Int32 MonthsBetweenOccasionalFilings { get; set; }
        public String OccasionalFilingCriteria { get; set; }
        public Int64 FormAggregationTypeId { get; set; }
        public Int64? PrepaymentCalculationMethodId { get; set; }
        public Boolean SplitOutOfStateLocations { get; set; }
        public String CompanyIdentifier { get; set; }
        public String PrepaymentHelpText { get; set; }
        public Int64? PrepaymentAccrualPeriodId { get; set; }
        public String PrepaymentThresholdCode { get; set; }
        public String CreditNetting { get; set; }
        public String AdditionalGrossOption { get; set; }
    }

}
