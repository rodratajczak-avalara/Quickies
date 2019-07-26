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

namespace ReportDueDates
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
            List<ReportData> reportDueDates  = new List<ReportData>();
            int recordsProcessed = 0;
            string filingMonth = DateTime.Parse(args[3].ToString() + "/" + args[4].ToString().PadLeft(2,'0') + "/" + "01").AddMonths(-1).ToString();
            Console.WriteLine("Obtaining Due Dates");

            // Exclude users that have not logged in within the last 120 days
            string sql = string.Format(@"SELECT fm.FormMasterId, fm.TaxFormCode, fm.Description, 
	                            Domains=STUFF((SELECT ',' + d.DomainName FROM Domain d WHERE d.DomainId IN (SELECT fd.DomainId FROM FormDomain fd WHERE fd.FormMasterId = fm.FormMasterId) FOR XML PATH, Type).value(N'.[1]', N'nvarchar(max)'), 1, 1, '')
                            FROM FormMaster fm
                            WHERE fm.IsEffective=1
                            AND (SELECT count(1) FROM FormDomain fd WHERE fd.FormMasterId = fm.FormMasterId AND  fd.DomainId IN (1, 2, 3, 4)) > 0
                            AND (SELECT count(1) FROM FormVersion fv WHERE fv.FormMasterId = fm.FormMasterId AND fv.Status='PRODUCTION' and fv.EffDate <= '{0}' and fv.EndDate >= '{1}') > 0 
                            ORDER BY TaxFormCode", filingMonth, filingMonth);

            using (TaxFormCatalogContext context = new TaxFormCatalogContext())
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ReportData reportDueDate = new ReportData
                    {
                        FormMasterId = (Int64)dataReader["FormMasterId"],
                        TaxFormCode = dataReader["TaxFormCode"].ToString(),
                        Description = dataReader["Description"].ToString(),
                        Domains = dataReader["Domains"].ToString(),
                        DueDate = GetDueDate(args[0].ToString(), args[1].ToString(), args[2].ToString(), (Int64)dataReader["FormMasterId"], Int32.Parse(args[3]), Int32.Parse(args[4]))
                    };
                    reportDueDates.Add(reportDueDate);
                    recordsProcessed++;
                    if (recordsProcessed % 1000 == 0)
                    {
                        Console.WriteLine(string.Format("Obtained due date for {0} forms.", recordsProcessed.ToString()));
                    }
                }
            }
            Console.WriteLine(string.Format("Obtained due date for {0} forms.", recordsProcessed.ToString()));
            Console.WriteLine("Generating Report");
            ExportList(reportDueDates, "DueDateReport.xlsx");
            Console.WriteLine(string.Format("DueDates for {0} forms have been written to DueDateReport.xlsx", reportDueDates.Count.ToString()));
            Console.ReadKey();
        }


        private static DateTime GetDueDate(string url, string Username, string Drowssap, Int64 FormMasterId, int Year, int Month)
        {
            DateTime dueDate = DateTime.Parse("1900-01-01");
            try
            {
                // create the http web request
                string query = string.Format("/api/FormDueDate?formMasterId={0}&month={1}&year={2}", FormMasterId, Month, Year);
                var request = (HttpWebRequest)WebRequest.Create(url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));

                // get response and deserialize it.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        List<FormDueDate> formDueDate = responseString.Length > 0 ? JsonConvert.DeserializeObject<List<FormDueDate>>(responseString) : null;
                        dueDate=formDueDate[0].DueDate;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("10: GetDueDate: unable to get due date for [{0}].  The unhandled exception that occurred: [{1}]", FormMasterId.ToString(), ex.Message);
            }

            return dueDate;
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
}
