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
using TaxFormCatalog.Shared.Entities;

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
        /// <remarks>args[4] = form master id to attach formCellDependencies</remarks>
        static void Main(string[] args)
        {
            List<InputData2> newDependencies = new List<InputData2>(); // = null;

            long updateFormMasterId = long.TryParse(args[4], out updateFormMasterId) ? updateFormMasterId : -1;

            if (args[0].Equals("d"))
            {
                List<FormCellDependency> currentDependencies = GetFormCellDependencies(updateFormMasterId, args[1], args[2], args[3]);
                Int64 deletedCount = 0;
                foreach (FormCellDependency fd in currentDependencies)
                {
                    DeleteFormCellDependency deleteResult = DeleteFormCellDependency(fd.FormCellDependencyId, args[1], args[2], args[3]);
                    if (deleteResult.Deleted)
                    {
                        deletedCount++;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Unable to delete formCellDependency [{0}] for FormMasterid [{1}].", fd.FormCellDependencyId.ToString(), updateFormMasterId.ToString()));
                    }
                }

                Console.WriteLine(string.Format("The application deleted [{0}] FormCellDependency records for FormMasterId [{1}].", deletedCount.ToString(), updateFormMasterId.ToString()));
            }
            else
            {
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
                            InputData2 newDependency = new InputData2();
                            int rowcolumn = 1;
                            int tempMonths = -1;
                            List<String> firstRow = new List<String>();

                            foreach (Cell c in row.Elements<Cell>())
                            {

                                switch (rowcolumn)
                                {
                                    //New Format
                                    case 1: newDependency.TaxFormCode = ReadExcelCell(c, workbookPart); break;
                                    case 2: newDependency.SummaryLabel = ReadExcelCell(c, workbookPart); break;
                                    case 3: newDependency.FieldName = ReadExcelCell(c, workbookPart) ?? null; break;
                                    case 4: newDependency.CurrentPeriod = ReadExcelCell(c, workbookPart) ?? null; break;
                                    case 5: newDependency.Months = int.TryParse(ReadExcelCell(c, workbookPart), out tempMonths) ? tempMonths : 0; break;
                                    case 6: newDependency.PriorYear = ReadExcelCell(c, workbookPart); break;
                                }

                                rowcolumn++;
                            }

                            if (newDependency.TaxFormCode != "Form")
                            {
                                newDependencies.Add(newDependency);
                            }
                        }
                    }
                }

                List<FormCellDependency> recordsToAdd = new List<FormCellDependency>();
                int totalRecords = newDependencies.Count;
                string prevTaxFormCode = string.Empty;
                long formMasterId = 0;
                List<FormCellDependency> existingDependencies = GetFormCellDependencies(updateFormMasterId, args[1], args[2], args[3]);
                List<Annotation> formAnnotations = new List<Annotation>();
                List<SummaryLabel> summaryLabels = GetSummaryLabels(args[1], args[2], args[3]);


                // Create List of FormCellDependency records to add 
                foreach (InputData2 item in newDependencies)
                {
                    if (!item.TaxFormCode.Equals(prevTaxFormCode))
                    {
                        formMasterId = GetFormMasterId(item.TaxFormCode, args[1], args[2], args[3]);
                        prevTaxFormCode = item.TaxFormCode;
                        if (formMasterId > 0)
                        {
                            formAnnotations = GetAnnotations(formMasterId, args[1], args[2], args[3]);
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Unable to find a formmaster record for taxformcode [{0}].", item.TaxFormCode));
                        }
                    }

                    if (formMasterId > 0)
                    {
                        try
                        {
                            FormCellDependency dependency = null;
                            Annotation annotation = formAnnotations.Any() ? formAnnotations.Where(a => a.AnnotationName == item.FieldName).FirstOrDefault() : null;
                            SummaryLabel summaryLabel = summaryLabels.Any() ? summaryLabels.Where(a => a.SummaryLabelCode == item.SummaryLabel || a.Description == item.SummaryLabel).FirstOrDefault() : null;
                            if (annotation == null)
                            {
                                if (summaryLabel == null)
                                {
                                    Console.WriteLine(string.Format("AddFormCellDependencyFromExcel:  Unable to find summary label [{0}] or field name [{1}] on tax form [{2}].", item.SummaryLabel, item.FieldName, item.TaxFormCode));
                                }
                                else
                                {
                                    dependency = existingDependencies.Any() ? existingDependencies.Where(d => d.SummaryLabelId == summaryLabel.SummaryLabelId && !d.AnnotationId.HasValue && d.FormMasterOptionId == formMasterId).FirstOrDefault() : null;
                                }
                            }
                            else
                            {
                                if (summaryLabel == null)
                                {
                                    dependency = existingDependencies.Any() ? existingDependencies.Where(d => d.AnnotationId == annotation.AnnotationId && !d.SummaryLabelId.HasValue && d.FormMasterOptionId == formMasterId).FirstOrDefault() : null;
                                }
                                else
                                {
                                    dependency = existingDependencies.Any() ? existingDependencies.Where(d => d.SummaryLabelId == summaryLabel.SummaryLabelId && d.AnnotationId == annotation.AnnotationId && d.FormMasterOptionId == formMasterId).FirstOrDefault() : null;
                                }
                            }
                            if (summaryLabel != null || annotation != null)
                            {
                                FormCellDependency dependencyToAdd = new FormCellDependency();
                                dependencyToAdd.FormCellDependencyId = dependency == null ? -1 : dependency.FormCellDependencyId;
                                dependencyToAdd.FormMasterId = updateFormMasterId;
                                dependencyToAdd.FormMasterOptionId = formMasterId;
                                dependencyToAdd.AnnotationId = annotation == null ? null : (long?)annotation.AnnotationId;
                                dependencyToAdd.SummaryLabelId = summaryLabel == null ? null : (long?)summaryLabel.SummaryLabelId; 
                                dependencyToAdd.IncludeCurrentPeriod = item.CurrentPeriod.ToUpper() == "1" ? true : false;
                                dependencyToAdd.MonthsAgo = item.Months;
                                dependencyToAdd.PriorYearEndFiscal = item.PriorYear.ToUpper() == "1" ? true : false;  
                                dependencyToAdd.CreatedDate = System.DateTime.UtcNow;
                                dependencyToAdd.CreatedUserId = 0;
                                dependencyToAdd.ModifiedDate = System.DateTime.UtcNow;
                                dependencyToAdd.ModifiedUserId = 0;
                                recordsToAdd.Add(dependencyToAdd);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("AddFormCellDependencyFromExcel: An error occurred [{0}] while processing TaxFormCode [{1}] and summary label [{2}] and field [{3}].", ex.Message, item.TaxFormCode, item.SummaryLabel, item.FieldName));
                        }

                    }
                }


                // insert records to FormCellDependency 
                int recordsInserted = 0;
                int recordsUpdated = 0;
                int recordsErrored = 0;
                if (recordsToAdd.Count > 0)
                {
                    recordsInserted = UpdateFormCellDepencencies(recordsToAdd, args[1], args[2], args[3], out recordsUpdated, out recordsErrored);
                }
                Console.WriteLine(string.Format("AddFormCellDependencyFromExcel: {0} records inserted and {1} records updated out of {2} total records.  There were {3} records that resulted in an error. ", recordsInserted.ToString(), recordsUpdated.ToString(), totalRecords.ToString(), recordsErrored.ToString()));
            }
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


        private static List<FormCellDependency> GetFormCellDependencies(long FormMasterId, string Url, string Username, string Drowssap)
        {
            List<FormCellDependency> results = new List<FormCellDependency>();

            try
            {
                string query = string.Format("/api/FormCellDependency?FormMasterId={0}", FormMasterId.ToString());

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


        private static DeleteFormCellDependency DeleteFormCellDependency(long FormCellDependencyId, string Url, string Username, string Drowssap)
        {
            DeleteFormCellDependency result = null;

            try
            {
                string query = string.Format("/api/FormCellDependency/{0}", FormCellDependencyId.ToString());

                // Create the http web request.
                var request = (HttpWebRequest)WebRequest.Create(Url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));
                request.Method = "DELETE";

                // execute the request.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        result = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<DeleteFormCellDependency>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteFormCellDependency: An unhandled exception occurred:[{0}]", ex.Message);

                return result;
            }

            return result;
        }


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
                Console.WriteLine("GetFormMasterId 1: Unable to find TaxFormCode [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);
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
                Console.WriteLine("GetFormMasterId 2: Unable to find LegacyReturnName [{0}] due to an unhandled exception:[{1}]", TaxFormCode, ex.Message);

                return formMasterId;
            }

            return formMasterId;
        }


        private static List<Annotation> GetAnnotations(long FormMasterId, string Url, string Username, string Drowssap)
        {
            List<Annotation> results = new List<Annotation>();

            try
            {
                string query = string.Format("/api/Annotation?formMasterId={0}", FormMasterId.ToString());

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
                Console.WriteLine("GetAnnotations: Attempting to get annotations for FormMasterId [{0}] resulted in an unhandled exception:[{0}]", FormMasterId.ToString(), ex.Message);

                return results;
            }

            return results;
        }

        private static List<SummaryLabel> GetSummaryLabels(string Url, string Username, string Drowssap)
        {
            List<SummaryLabel> results = new List<SummaryLabel>();

            try
            {
                string query = string.Format("/api/SummaryLabel/");

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

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<SummaryLabel>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSummaryLabels: An unhandled exception occurred:[{0}]", ex.Message);

                return results;
            }

            return results;
        }

        private static Int32 UpdateFormCellDepencencies(List<FormCellDependency> dependencyList, string url, string Username, string Drowssap, out int numberUpdated, out int numberErrored)
        {
            int numberInserted = 0;
            numberUpdated = 0;
            numberErrored = 0;

            foreach (FormCellDependency currentDependency in dependencyList)
            {
                string payload = string.Empty;
                try
                {
                    // get post data.
                    payload = JsonConvert.SerializeObject(currentDependency);
                    byte[] buf = Encoding.UTF8.GetBytes(payload);

                    // create the http web request
                    string query = currentDependency.FormCellDependencyId > 0 ? string.Format("/api/FormCellDependency/{0}", currentDependency.FormCellDependencyId.ToString()) : "/api/FormCellDependency";
                    var request =  (HttpWebRequest)WebRequest.Create(url + query);
                    request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                    request.Method = currentDependency.FormCellDependencyId > 0 ? "PUT" : "POST";
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
                            if (currentDependency.FormCellDependencyId > 0)
                            {
                                if (responseDependency.FormCellDependencyId > 0)
                                {
                                    numberUpdated++;
                                }
                            }
                            else
                            {
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
                    Console.WriteLine("UpdateFormCellDepencencies: The following attempted insert/update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
                    numberErrored++;
                }
            }

            return numberInserted;
        }

    }

    public class InputData2
    {
        public String TaxFormCode { get; set; }
        public String SummaryLabel { get; set; }
        public String FieldName { get; set; }
        public String CurrentPeriod { get; set; }
        public int Months { get; set; }
        public String PriorYear { get; set; }
    }

    public class DeleteFormCellDependency
    {
        public String Name { get; set; }
        public Int64 FormCellDependencyId { get; set; }
        public Boolean Deleted { get; set; }
    }

}
