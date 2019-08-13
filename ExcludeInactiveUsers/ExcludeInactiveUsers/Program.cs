using System;
using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExcludeInactiveUsers
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


            // Exclude users that have not logged in within the last 120 days
            string sqlExclude = @"SELECT * 
                                FROM CatalogUser
                                WHERE Role <> 'EXCLUDED'
                                AND LastLoginDate <= DateAdd(d, -120, getdate())";
            List<CatalogUser> catalogUsers = new List<CatalogUser>();
            int numberExcluded = 0;

            using (var context = new TaxFormCatalogContext())
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sqlExclude;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string updateSQL = @"UPDATE FormMaster 
                                    SET DORAddress1 = DORAddress2, DORAddress2 = null
                                    WHERE TaxFormCode = '" + dataReader["TaxFormCode"].ToString() + "'";
                    
                    
                    CatalogUser catalogUser = new CatalogUser
                    {
                        CatalogUserId = (Int64)dataReader["CatalogUserId"],
                        IdentityGuid = dataReader["IdentityGuid"].ToString(),
                        UserName = dataReader["UserName"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        CreatedDate = (DateTime)dataReader["CreatedDate"],
                        ModifiedDate = (DateTime)dataReader["ModifiedDate"],
                        Role = dataReader["Role"].ToString(),
                        Passphrase = dataReader["Passphrase"].ToString(),
                        LastLoginDate = (DateTime)dataReader["LastLoginDate"],
                        ProductionNotify = (Boolean)dataReader["ProductionNotify"],
                        Email = dataReader["Email"].ToString()
                    };

                    Boolean result = UpdateUserRole(catalogUser, "EXCLUDED", args[0], args[1], args[2]);
                    if (!result)
                    {
                        Console.WriteLine(string.Format("Unable to Exclude CatalogUser {0}", catalogUser.UserName));
                    }
                    else
                    {
                        numberExcluded++;
                    }

                    if (numberExcluded % 10 == 0)
                    {
                        Console.WriteLine(string.Format("{0} users Excluded from Tax Form Catalog.", numberExcluded.ToString()));
                    }
                }

                Console.WriteLine(string.Format("{0} total users excluded in Tax Form Catalog.", numberExcluded.ToString()));

                cmd.Connection.Close();
            }

            // Re-enable users that have been excluded but logged in within the last 30 days
            string strSqlReadOnly = @"SELECT * 
                                FROM CatalogUser
                                WHERE Role = 'EXCLUDED'
                                AND LastLoginDate >= DateAdd(d, -30, getdate())";
            int numberReenabled = 0;

            using (var context = new TaxFormCatalogContext())
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = strSqlReadOnly;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    CatalogUser catalogUser = new CatalogUser
                    {
                        CatalogUserId = (Int64)dataReader["CatalogUserId"],
                        IdentityGuid = dataReader["IdentityGuid"].ToString(),
                        UserName = dataReader["UserName"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        CreatedDate = (DateTime)dataReader["CreatedDate"],
                        ModifiedDate = (DateTime)dataReader["ModifiedDate"],
                        Role = dataReader["Role"].ToString(),
                        Passphrase = dataReader["Passphrase"].ToString(),
                        LastLoginDate = (DateTime)dataReader["LastLoginDate"],
                        ProductionNotify = (Boolean)dataReader["ProductionNotify"],
                        Email = dataReader["Email"].ToString()
                    };

                    Boolean result = UpdateUserRole(catalogUser, "READONLY", args[0], args[1], args[2]);
                    if (!result)
                    {
                        Console.WriteLine(string.Format("Unable to Re-enable CatalogUser {0}", catalogUser.UserName));
                    }
                    else
                    {
                        numberReenabled++;
                    }

                    if (numberExcluded % 10 == 0)
                    {
                        Console.WriteLine(string.Format("{0} users re-enabled in Tax Form Catalog.", numberReenabled.ToString()));
                    }
                }

                Console.WriteLine(string.Format("{0} total users re-enabled in Tax Form Catalog.", numberReenabled.ToString()));

                cmd.Connection.Close();
            }

        }

        private static Boolean UpdateUserRole(CatalogUser catalogUser, String newRole, string Url, string Username, string Drowssap)
        {
            try
            {
                string query = string.Format("/api/CatalogUser/{0}", catalogUser.CatalogUserId.ToString());
                catalogUser.Role = newRole;
                catalogUser.ModifiedDate = System.DateTime.UtcNow;
                catalogUser.ProductionNotify = false;

                // Create the http web request.
                var request = (HttpWebRequest)WebRequest.Create(Url + query);
                request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Username + ":" + Drowssap)));
                request.Method = "PUT";
                request.ContentType = "application/json";
                var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(catalogUser));
                var newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                // execute the request.
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null)
                    {
                        var responseStream = response.GetResponseStream();
                        var streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
                        var responseString = streamReader.ReadToEnd();

                        CatalogUser apiResponse = responseString.Length > 0 ? Newtonsoft.Json.JsonConvert.DeserializeObject<CatalogUser>(responseString) : null;
                        if (apiResponse != null)
                        {
                            return true;
                        }
                        Console.WriteLine("ExcludeUser: Invalid API Response:[NULL]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteFormDueDate: An unhandled exception occurred:[{0}]", ex.Message);

                return false;
            }

            return false;
        }

        
    }
}


public class CatalogUser
{
    public Int64 CatalogUserId { get; set; }
    public String IdentityGuid { get; set; }
    public String UserName { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public String Role { get; set; }
    public String Passphrase { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public Boolean ProductionNotify { get; set; }
    public String Email { get; set; }
}



