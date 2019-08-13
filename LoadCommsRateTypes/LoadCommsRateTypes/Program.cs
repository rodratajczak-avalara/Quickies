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

namespace UpdateTFCHolidays
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
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    int rowNumber = 1;
                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        InputData filingSystem = new InputData();
                        int rowcolumn = 1;

                        if (rowNumber > 1)
                        {
                            foreach (Cell c in row.Elements<Cell>())
                            {
                                switch (rowcolumn)
                                {
                                    case 1: filingSystem.CountryCode = ReadExcelCell(c, workbookPart); break;
                                    case 2: filingSystem.State = ReadExcelCell(c, workbookPart); break;
                                    case 3: filingSystem.HolidayDate = DateTime.FromOADate(Convert.ToInt32(ReadExcelCell(c, workbookPart))); break;
                                    case 4: filingSystem.HolidayName = ReadExcelCell(c, workbookPart); break;
                                }
                                rowcolumn++;
                            }
                            filingSystems.Add(filingSystem);
                        }
                        rowNumber++;
                    }
                }
            }

            int totalRecords = filingSystems.Count;
            int recordsInserted = 0;
            int recordsErrored = 0;
            int recordsSkipped = 0;
            List<Holiday> holidays = GetHolidays(args[1], args[2], args[3]);

            foreach (InputData item in filingSystems)
            {
                List<Holiday> foundHolidays = holidays.Where(x => x.Country == item.CountryCode.Substring(0,2) && x.Region == item.State && x.Date == item.HolidayDate).ToList();
                if (!foundHolidays.Any())
                {
                    Holiday holidayToAdd = new Holiday();
                    holidayToAdd.Country = item.CountryCode.Substring(0, 2);
                    holidayToAdd.Region = item.State;
                    holidayToAdd.BankId = 1;
                    holidayToAdd.Description = item.HolidayName;
                    holidayToAdd.Date = item.HolidayDate;

                    Int64 insertedHolidayId = -1;
                    insertedHolidayId = InsertHoliday(holidayToAdd, args[1], args[2], args[3]);
                    if (insertedHolidayId > 0)
                    {
                        recordsInserted++;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("2: Insert to Holiday did not return a valid HolidayId for Country [{0}], Region [{1}], Holiday Name [{2}] with date of [{3}].", item.CountryCode, item.State, item.HolidayName, item.HolidayDate));
                        recordsErrored++;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("3: Insert to Holiday was skipped because a record for the same country,region, and date already exists. Country [{0}], Region [{1}], Holiday Name [{2}] with date of [{3}].", item.CountryCode, item.State, item.HolidayName, item.HolidayDate));
                    recordsSkipped++;
                }
            }

            Console.WriteLine(string.Format("4: Process Completed with [{0}] records inserted, [{1}] records skipped, [{2}] records Errored while processing [{3}] total records.", recordsInserted, recordsSkipped, recordsErrored, totalRecords));
            Console.ReadKey();
        }

        /// <summary>
        /// obtain the value from a cell in excel
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
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


        private static List<Holiday> GetHolidays(string Url, string Username, string Drowssap)
        {
            List<Holiday> results = new List<Holiday>();

            try
            {
                string query = "/api/Holiday";

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

                        results = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Holiday>>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("6: GetHolidays: An unhandled exception occurred:[{0}]", ex.Message);

                return results;
            }

            return results;
        }

        private static Int64 InsertHoliday(Holiday holiday, string url, string Username, string Drowssap)
        {
            string payload = string.Empty;
            Int64 holidayId = -1;
            try
            {
                // get post data.
                payload = JsonConvert.SerializeObject(holiday);
                byte[] buf = Encoding.UTF8.GetBytes(payload);

                // create the http web request
                string query = "/api/Holiday";
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

                        Holiday responseHoliday = responseString.Length > 0 ? JsonConvert.DeserializeObject<Holiday>(responseString) : null;
                        holidayId = responseHoliday.HolidayId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("9: InsertHoliday: The following attempted insert/update failed: [{0}].  The unhandled exception that occurred: [{1}]", payload, ex.Message);
            }

            return holidayId;
        }

    }

    public class InputData
    {
        public String CountryCode { get; set; }
        public String State { get; set; }
        public DateTime HolidayDate { get; set; }
        public String HolidayName { get; set; }
    }

    public class Holiday
    {
        public Int64 HolidayId { get; set; }
        public Int64 BankId { get; set; }
        public String Description { get; set; }
        public String FrequencyNotes { get; set; }
        public String Country { get; set; }
        public String Region { get; set; }
        public DateTime Date { get; set; }
        public Int64 CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
