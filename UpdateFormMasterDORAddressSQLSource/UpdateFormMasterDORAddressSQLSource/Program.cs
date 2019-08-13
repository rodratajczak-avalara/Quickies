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
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UpdateFormMasterDORAddress
{
    class Program
    {
        private readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        static void Main(string[] args)
        {
            Console.WriteLine("Beginning Processing");
            int recordsUpdated = 0;
            int recordsErrored = 0;

            // Exclude users that have not logged in within the last 120 days
            string sql = @"SELECT *
                            FROM FormMaster fm
                            WHERE (fm.DORAddress1 is NULL or fm.DORAddress1 = '') 
                            AND (fm.DORAddress2 is NOT NULL or fm.DORAddress2 != '')";

            using (TaxFormCatalogContext context = new TaxFormCatalogContext())
            {
                List<FormMaster> formMasters = FillList(context, sql);

                foreach (FormMaster formMaster in formMasters)
                {
                    formMaster.DORAddress1 = formMaster.DORAddress2;
                    formMaster.DORAddress2 = null;
                    bool success = UpdateFormMasterDORAddress(formMaster, args[0], args[1], args[2]);
                    if (success)
                    {
                        Console.WriteLine(string.Format("3: UpdateFormMasterAddress: TaxFormCode [{0}] was successfully updated.", formMaster.TaxFormCode));
                        recordsUpdated++;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("4: UpdateFormMasterAddress: TaxFormCode [{0}] was not updated.", formMaster.TaxFormCode));
                        recordsErrored++;
                    }

                }

                Console.WriteLine(string.Format("7: UpdateFormMasterDORAddresses: {0} records updated out of {1} total records.  There were {2} records that resulted in an error.", recordsUpdated.ToString(), formMasters.Count.ToString(), recordsErrored.ToString()));
                Console.WriteLine("Complete");
            }
            Console.ReadKey();
        }

        public static List<FormMaster> FillList(TaxFormCatalogContext context, string SQLCommand) 
        {
            var list = new List<FormMaster>();

            //Load the data into the SqlDataReader
            using (var dataCommand = context.Database.GetDbConnection().CreateCommand())
            {
                dataCommand.CommandType = System.Data.CommandType.Text;
                dataCommand.CommandText = SQLCommand;

                if (dataCommand.Connection.State != System.Data.ConnectionState.Open)
                {
                    dataCommand.Connection.Open();
                }

                var dataReader = dataCommand.ExecuteReader();

                //Fill the list with the contents of the reader
                while (dataReader.Read())
                {
                    var obj = new FormMaster();

                    //Get the property information
                    var properties = typeof(FormMaster).GetProperties();
                    int i = 0;

                    foreach (var property in properties)
                    {
                        property.SetValue(obj, dataReader[i], null); // set the fields of T to the reader's value
                        i++;
                    }

                    list.Add(obj);
                }

                dataReader.Close();
            }

            return list;
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
                string query = "/api/FormMaster/" + formMaster.FormMasterId.ToString();
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
                Console.WriteLine("10: UpdateFormMasterDORAddress: The following attempted update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return success;
        }

    }

    public class FormMaster
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
