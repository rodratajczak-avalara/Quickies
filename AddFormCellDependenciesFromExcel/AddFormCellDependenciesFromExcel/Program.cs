using System;
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

namespace AddFormCellDependenciesFromExcel
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
            List<InputData> dependencies = null;

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
                        InputData dependency = new InputData();
                        int rowcolumn = 1;
                        foreach (Cell c in row.Elements<Cell>())
                        {
                            switch (rowcolumn)
                            {
                                case 1: dependency.TaxFormCode = c.CellValue.Text; break;
                                case 2: dependency.CurrentPeriod = c.CellValue.Text; break;
                                case 3: dependency.Months = int.Parse(c.CellValue.Text); break;
                                case 4: dependency.OriginCellName = c.CellValue.Text; break;
                                case 5: dependency.DestinationCellName = c.CellValue.Text; break;
                                case 6: dependency.CityCountyCode = c.CellValue.Text; break;
                                case 7: dependency.TaxType = c.CellValue.Text; break;
                                case 8: dependency.Status = c.CellValue.Text; break;
                                case 9: dependency.Notes = c.CellValue.Text; break;
                            }

                            dependencies.Add(dependency);
                            rowcolumn++;
                        }
                    }
                }
            }

            List<FormCellDependency> recordsToAdd = null;
            int totalRecords = dependencies.Count;

            // Create List of FormCellDependency records to add 
            foreach (InputData item in dependencies)
            {


            }

            int recordsAlreadyInserted = totalRecords - recordsToAdd.Count;

            // insert records to FormCellDependency 
            int recordsInserted = UpdateFormCellDepencencies(recordsToAdd, args[1], args[2], args[3]);
            
            Console.WriteLine(string.Format("AddFormCellDependencyFromExcel: {0} records inserted out of {1} total records.  {2} records already existed and were not added.", recordsInserted.ToString(), totalRecords.ToString(), recordsAlreadyInserted.ToString()));

        }


        private static List<FormCellDependency> GetFormCellDependencies(string TaxFormCode, string Url, string Username, string Drowssap)
        {
            List<FormCellDependency> results = null;

            try
            {
                string query = string.Format("/api/FormCellDependency?taxFormCode={0}", TaxFormCode);

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

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<FormCellDependency>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFormCellDependencies: An unhandled exception occurred:[{0}]", ex.Message);

                return results;
            }

            return results;
        }


        private static Int64 GetFormMasterId(string TaxFormCode, string Url, string Username, string Drowssap)
        {
            Int64 formMasterId = -1;
            FormMaster formMaster;

            try
            {
                string query = string.Format("/api/FormMaster/%7Bid%7D?taxFormCode={0}", TaxFormCode);

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

                        formMaster = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<FormMaster>(responseString) : null;
                        formMasterId = formMaster.FormMasterId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFormMasterId: An unhandled exception occurred:[{0}]", ex.Message);

                return formMasterId;
            }

            return formMasterId;
        }


        private static List<Annotation> GetAnnotations(string TaxFormCode, string Url, string Username, string Drowssap)
        {
            List<Annotation> results = null;

            try
            {
                string query = string.Format("/api/FormMaster/%7Bid%7D?taxFormCode={0}", TaxFormCode);

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

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Annotation>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAnnotations: An unhandled exception occurred:[{0}]", ex.Message);

                return results;
            }

            return results;
        }

        private static Int32 UpdateFormCellDepencencies(List<FormCellDependency> dependencyList, string url, string Username, string Drowssap)
        {
            int numberInserted = 0;

            try
            {
                string query = "/api/FormCellDependency";

                foreach (FormCellDependency currentDependency in dependencyList)
                {
                    // get post data.
                    string payload = JsonConvert.SerializeObject(currentDependency);
                    byte[] buf = Encoding.UTF8.GetBytes(payload);

                    // create the http web request
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

                            FormCellDependency responseDependency = responseString.Length > 0 ? JsonConvert.DeserializeObject<FormCellDependency>(responseString) : null;
                            if (responseDependency.FormCellDependencyId > 0)
                            {
                                numberInserted++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateFormCellDepencencies: An unhandled exception occurred:[{0}]", ex.Message);

                return numberInserted;
            }

            return numberInserted;
        }

    }

    public class InputData
    {
        public String TaxFormCode { get; set; }
        public String CurrentPeriod { get; set; }
        public int Months { get; set; }
        public String OriginCellName { get; set; }
        public String DestinationCellName { get; set; }
        public String CityCountyCode { get; set; }
        public String TaxType { get; set; }
        public String Status { get; set; }
        public String Notes { get; set; }
    }

    public class FormCellDependency
    {
        public Int64 FormCellDependencyId { get; set; }
        public Int64 FormMasterId { get; set; }
        public Int64? SummaryLabelId { get; set; }
        public Int64 FormMasterOptionId { get; set; }
        public Boolean IncludeCurrentPeriod { get; set; }
        public Int32 MonthsAgo { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean PriorYearEndFiscal { get; set; }
        public Int64? AnnotationId { get; set; }
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


    public partial class Annotation
    {
        public Int64 AnnotationId { get; set; }
        public Int64 FormVersionId { get; set; }
        public String AnnotationName { get; set; }
        public Boolean ReadOnly { get; set; }
        public String JsFormat { get; set; }
        public Int64? ImportPathId { get; set; }
        public String FullName { get; set; }
        public String Calculation { get; set; }
        public String Validation { get; set; }
        public String FormatView { get; set; }
        public String Description { get; set; }
        public String DefaultValue { get; set; }
        public Boolean EmptyIfZero { get; set; }
        public String CommonName { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public String GroupCode { get; set; }
        public String ReportLevel { get; set; }
        public String DataType { get; set; }
        public Boolean? PdfFormatPercent { get; set; }
        public Int64? PdfFormatDecimalPlaces { get; set; }
        public Boolean? PdfFormatUseSeparator { get; set; }
        public String PdfFormatDateMask { get; set; }
        public String TaxTypeIds { get; set; }
        public String RateTypeIds { get; set; }
        public String DeductionTypeIds { get; set; }
        public Int64? SummaryLabelId { get; set; }
        public String JurisName { get; set; }
        public Boolean IsHidden { get; set; }
        public Int64? PositionHeaderId { get; set; }
        public String NegativeFormat { get; set; }
        public Boolean RemoveDecimal { get; set; }
        public Boolean NegateTaxTypes { get; set; }
        public Boolean NegateRateTypes { get; set; }
        public Boolean NegateDeductionTypes { get; set; }
        public Boolean NegateGroupCode { get; set; }
        public Boolean NegateJurisName { get; set; }
        public Boolean NegateReportLevel { get; set; }
        public Boolean HiddenFromCompliance { get; set; }
        public String JsonData { get; set; }
        public String ValidationSeverity { get; set; }
        public String ValidationMessage { get; set; }
        public Int64 PdfFormatDecimal { get; set; }
        public Int64 PdfFormatSeparator { get; set; }
        public Boolean Recalculate { get; set; }
        public String Instruction { get; set; }
        public Int32? MaxLength { get; set; }
    }


}
