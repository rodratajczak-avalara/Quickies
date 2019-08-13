using System;
using System.IO;
using System.Net;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeleteFormDueDate
{
    class Program
    {
        private readonly IConfiguration _config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .Build();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>args[0] = url for Tax Form Catalog</remarks>
        /// <remarks>args[1] = user id for Tax Form Catalog</remarks>
        /// <remarks>args[2] = password for Tax Form Catalog</remarks>
        static void Main(string[] args)
        {
            using (TaxFormCatalogContext tfcContext = new TaxFormCatalogContext())
            {
                using (DbCommand cmdTables = tfcContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdTables.CommandTimeout = 30;
                    cmdTables.CommandType = CommandType.Text;
                    cmdTables.CommandText = @"SELECT FormDueDateId
                                              FROM FormDueDate
                                              WHERE ModifiedUserId IN (338, 494, 70, 568)";
                    if (cmdTables.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdTables.Connection.Open();
                    }

                    using (DbDataReader readerTables = cmdTables.ExecuteReader())
                    {
                        long _formDueDateId = 0;
                        long _numberDeleted = 0;
                        while (readerTables.Read())
                        {
                            _formDueDateId = (long)readerTables["FormDueDateId"];
                            FormDueDateDelete removed = DeleteFormDueDate(_formDueDateId, args[0], args[1], args[2]);
                            if (!removed.Deleted)
                            {
                                Console.WriteLine(string.Format("Unable to process Delete for formDueDateId {0}", _formDueDateId.ToString()));
                            }
                            else
                            {
                                _numberDeleted++;
                            }

                            if (_numberDeleted % 100 == 0)
                            {
                                Console.WriteLine(string.Format("{0} records deleted from FormDueDate.", _numberDeleted.ToString()));
                            }
                        }
                    }
                    cmdTables.Connection.Close();
                }
            }
        }

        private static FormDueDateDelete DeleteFormDueDate(long formDueDateId, string Url, string Username, string Drowssap)
        {
            FormDueDateDelete result = null;
            try
            {
                string query = string.Format("/api/FormDueDate/{0}", formDueDateId.ToString());

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

                        result = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<FormDueDateDelete>(responseString) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteFormDueDate: An unhandled exception occurred:[{0}]", ex.Message);

                return result;
            }

            return result;
        }
    }
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

public partial class FormDueDateDelete
{
    public Int64 Id { get; set; }
    public Boolean Deleted { get; set; }
}


