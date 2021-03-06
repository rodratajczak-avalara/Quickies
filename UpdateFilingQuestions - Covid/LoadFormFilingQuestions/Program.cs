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

namespace LoadFormFilingQuestionRecords
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
            List<InputData> formList = new List<InputData>();

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
                        InputData form = new InputData();
                        int rowcolumn = 1;

                        foreach (Cell c in row.Elements<Cell>())
                        {

                            switch (rowcolumn)
                            {
                                case 1: form.State = ReadExcelCell(c, workbookPart); break;
                                case 2: form.TaxFormCode = ReadExcelCell(c, workbookPart); break;
                                case 3: form.FormInfo = ReadExcelCell(c, workbookPart); break;
                                case 4: form.LucasNotes = ReadExcelCell(c, workbookPart); break;
                            }

                            rowcolumn++;
                        }

                        formList.Add(form);
                    }
                }
            }

            int totalRecords = formList.Count;
            Int64 formMasterId = 0;
            int recordsInserted = 0;
            int recordsUpdated = 0;
            int recordsDeleted = 0;
            int recordsErrored = 0;

            // Create List of FormCellDependency records to add 
            foreach (InputData form in formList)
            {
                formMasterId = GetFormMasterId(form.TaxFormCode, args[1], args[2], args[3]);
                if (formMasterId <= 0)
                {
                    Console.WriteLine(string.Format("1: Unable to find a formmaster record for taxformcode [{0}].", form.TaxFormCode));
                }
                else
                {
                    try
                    {

                        List<FormFilingQuestion> existingFilingQuestions = GetFormFilingQuestions(formMasterId, args[1], args[2], args[3]);
                        FormFilingQuestion formFilingQuestion = existingFilingQuestions.Any() ? existingFilingQuestions.Where<FormFilingQuestion>(x => x.FilingQuestionId == 1272).FirstOrDefault() : null;
                        if (formFilingQuestion == null || formFilingQuestion == new FormFilingQuestion())
                        {
                            FormFilingQuestion filingQuestionToAdd = new FormFilingQuestion()
                            {
                                FormFilingQuestionId = -1,
                                FormMasterId = formMasterId,
                                SortOrder = existingFilingQuestions.Count + 1,
                                FilingQuestionId = 1272,
                                Required = false,
                                InternalOnly = false,
                                FormQuestionScopeId = 0,
                                SkyscraperValidationRequired = false
                            };

                            Int64 insertedFilingQuestionId = -1;
                            insertedFilingQuestionId = InsertFormFilingQuestion(filingQuestionToAdd, args[1], args[2], args[3]);
                            if (insertedFilingQuestionId > 0)
                            {
                                recordsInserted++;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("2: Insert to FormFilingQuestion did not return a valid FormFilingQuestionId for TaxFormCode [{0}].", form.TaxFormCode));
                                recordsErrored++;
                            }

                        }
                        /*                        else // set InternalOnly to True to remove from CUP
                                                {
                                                    formFilingQuestion.InternalOnly = true;
                                                    Int64 updateFilingQuestionId = -1;
                                                    updateFilingQuestionId = UpdateFormFilingQuestion(formFilingQuestion, args[1], args[2], args[3]);
                                                    if (updateFilingQuestionId > 0)
                                                    {
                                                        recordsUpdated++;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(string.Format("3: Update of existing FormFilingQuestion did not return a valid FormFilingQuestionId for TaxFormCode [{0}].", form.TaxFormCode));
                                                        recordsErrored++;
                                                    }
                                                }
                        */
                        else // remove the filing question from the form
                        {
                            Int64 deleteFilingQuestionId = -1;
                            deleteFilingQuestionId = DeleteFormFilingQuestion(formFilingQuestion, args[1], args[2], args[3]);
                            if (deleteFilingQuestionId >= 0)
                            {
                                recordsDeleted++;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("3: Update of existing FormFilingQuestion did not return a valid FormFilingQuestionId for TaxFormCode [{0}].", form.TaxFormCode));
                                recordsErrored++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("4: LoadFormFilingQuestion: An error occurred [{0}] while processing TaxFormCode [{1}] and filing Question [{2}].", ex.Message, form.TaxFormCode, "1372"));
                        recordsErrored++;
                    }

                }
            }

            Console.WriteLine(string.Format("5: LoadFormFilingQuestion: {0} records inserted, {1} records updated, {2} records deleted and {3} records errored out of {4} total records.", recordsInserted.ToString(), recordsUpdated.ToString(), recordsDeleted.ToString(), recordsErrored.ToString(), totalRecords.ToString()));
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
        /// obtains the list of FormFilingQuestions for a given FormMaster
        /// </summary>
        /// <param name="FormMasterId"></param>
        /// <param name="Url"></param>
        /// <param name="Username"></param>
        /// <param name="Drowssap"></param>
        /// <returns></returns>
        private static List<FormFilingQuestion> GetFormFilingQuestions(long FormMasterId, string Url, string Username, string Drowssap)
        {
            List<FormFilingQuestion> results = new List<FormFilingQuestion>();

            try
            {
                string query = string.Format("/api/FormFilingQuestion?FormMasterId={0}", FormMasterId.ToString());

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

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<FormFilingQuestion>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("6: GetFormFilingQuestion: An unhandled exception occurred:[{0}]", ex.Message);

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

            try
            {
                // Create the http web request.
                var request1 = (HttpWebRequest)WebRequest.Create(Url + string.Format("/api/FormMaster/%7Bid%7D?taxFormCode={0}", TaxFormCode));
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
                    var request2 = (HttpWebRequest)WebRequest.Create(Url + string.Format("/api/FormMaster/%7Bid%7D?legacyReturnName={0}", TaxFormCode));
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

        private static Int64 InsertFormFilingQuestion(FormFilingQuestion filingQuestion, string url, string Username, string Drowssap)
        {
            string payload = string.Empty;
            Int64 formFilingQuestionId = -1;
            try
            {
                // get post data.
                payload = JsonConvert.SerializeObject(filingQuestion);
                byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/FormFilingQuestion";
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

                        FormFilingQuestion responseDependency = responseString.Length > 0 ? JsonConvert.DeserializeObject<FormFilingQuestion>(responseString) : null;
                        formFilingQuestionId = responseDependency.FormFilingQuestionId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("9: InsertFormFilingQuestion: The following attempted insert failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return formFilingQuestionId;
        }

        private static Int64 UpdateFormFilingQuestion(FormFilingQuestion filingQuestion, string url, string Username, string Drowssap)
        {
            string payload = string.Empty;
            Int64 formFilingQuestionId = -1;
            try
            {
                // get post data.
                payload = JsonConvert.SerializeObject(filingQuestion);
                byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/FormFilingQuestion/" + filingQuestion.FormFilingQuestionId.ToString();
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

                        FormFilingQuestion responseDependency = responseString.Length > 0 ? JsonConvert.DeserializeObject<FormFilingQuestion>(responseString) : null;
                        formFilingQuestionId = responseDependency.FormFilingQuestionId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("10: UpdateFormFilingQuestion: The following attempted update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return formFilingQuestionId;
        }




        private static Int64 DeleteFormFilingQuestion(FormFilingQuestion filingQuestion, string url, string Username, string Drowssap)
        {
            //string payload = string.Empty;
            Int64 formFilingQuestionId = -1;
            try
            {
                // get post data.
                //payload = JsonConvert.SerializeObject(filingQuestion);
                //byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/FormFilingQuestion/" + filingQuestion.FormFilingQuestionId.ToString();
                var request = (HttpWebRequest)WebRequest.Create(url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                request.Method = "DELETE";
                //request.ContentLength = buf.Length;
                //request.ContentType = "application/json; charset=utf-8";
                //request.GetRequestStream().Write(buf, 0, buf.Length);

                // get response and deserialize it.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        DeleteResponse responseDependency = responseString.Length > 0 ? JsonConvert.DeserializeObject<DeleteResponse>(responseString) : null;
                        if (responseDependency.deleted)
                        {
                            formFilingQuestionId = responseDependency.id;
                        }
                        else
                        {
                            Console.WriteLine("12: DeleteFormFilingQuestion: The following attempted delete failed: [{0}].  The unhandled exception that occurred: [{1}]", responseDependency.id.ToString(), responseDependency.deleted.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("11: DeleteFormFilingQuestion: The following attempted delete failed: [{0}].  The unhandled exception that occurred: [{1}]", filingQuestion.FormFilingQuestionId.ToString(), ex.Message);
            }

            return formFilingQuestionId;
        }
    }


    public class InputData
    {
        public String State { get; set; }
        public String TaxFormCode{ get; set; }
        public String FormInfo { get; set; }
        public String LucasNotes { get; set; }
    }

    public class DeleteResponse
    {
        public string name { get; set; }
        public Int64 id { get; set; }
        public Boolean deleted { get; set; }
    }

    public class FormFilingQuestion
    {
        public Int64 FormFilingQuestionId { get; set; }
        public Int64 FormMasterId { get; set; }
        public Int32 SortOrder { get; set; }
        public Int64 FilingQuestionId { get; set; }
        public Boolean Required { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean InternalOnly { get; set; }
        public Int64 FormQuestionScopeId { get; set; }
        public Boolean SkyscraperValidationRequired { get; set; }
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
        public Boolean MarketplaceTransactionsAsDeductions { get; set; }
        public Boolean ReadyToPrep { get; set; }
        public DateTime? ExpectedReleaseDate { get; set; }
        public String InternalNotes { get; set; }
        public String ExternalNotes { get; set; }
        public String AssociatedTicketUrl { get; set; }
        public Boolean AllowFilingCalendarAutoApproval { get; set; }
        public Boolean AllowFilingWithoutPayment { get; set; }
        public Boolean GrossFromDetail { get; set; }
        public String DORFaxNumber { get; set; }
        public String DOREmailAddress { get; set; }
        public String ZeroPhoneNumber { get; set; }
        public String ZeroFaxNumber { get; set; }
        public String ZeroWebsite { get; set; }
        public String ZeroEmailAddress { get; set; }
        public String AmendedPhoneNumber { get; set; }
        public String AmendedFaxNumber { get; set; }
        public String AmendedWebsite { get; set; }
        public String AmendedEmailAddress { get; set; }
        public String PaymentAddressMailTo { get; set; }
        public String PaymentAddress1 { get; set; }
        public String PaymentAddress2 { get; set; }
        public String PaymentAddressCity { get; set; }
        public String PaymentAddressRegion { get; set; }
        public String PaymentAddressPostalCode { get; set; }
        public String PaymentAddressCountry { get; set; }
        public String PaymentPhoneNumber { get; set; }
        public String PaymentFaxNumber { get; set; }
        public String PaymentWebsite { get; set; }
        public String PaymentEmailAddress { get; set; }
        public String FormNotesHyperlink { get; set; }
    }


}
