
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace AvaShardAggregator
{
    internal class AggregateShardService
    {
        private static IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        private static string objectSuffix = config.GetSection("ObjectSuffix").Value;

        public AggregateShardService() 
        {
            DateTime lastSynch = GetLastSynch();
            DateTime startTime = DateTime.UtcNow;

            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("Source")))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Document WHERE ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", lastSynch);
                    cmd.Parameters.AddWithValue("@CurrentCheckTime", startTime);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        PerformBulkCopy(reader);
                    }
                }
            }


            DateTime endTime = DateTime.UtcNow;
            UpdateLastSynch(startTime);

            // Log Total Processing Time
            Console.WriteLine(string.Format("Total Processing Time: {0}", endTime.Subtract(startTime).TotalMilliseconds).ToString());
            Console.ReadKey();
        }

        private static void PerformBulkCopy(SqlDataReader r)
        {
            DateTime startBCP;
            DateTime endBCP;
            DateTime startMerge;
            DateTime endMerge;

            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("Destination")))
            {
                conn.Open();

                //Bulk copy the data from the source
                startBCP = DateTime.UtcNow;
                using (SqlBulkCopy cpy = new SqlBulkCopy(conn))
                {
                    cpy.BulkCopyTimeout = 3600;
                    cpy.DestinationTableName = string.Format("Document{0}", objectSuffix);
                    cpy.WriteToServer(r);
                }
                endBCP = DateTime.UtcNow;

                // Perform Merge of Data
                startMerge = DateTime.UtcNow;
                using (SqlCommand cmdMerge = new SqlCommand(string.Format("sp_Merge_Document_From{0}", objectSuffix), conn))
                {
                    cmdMerge.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdMerge.CommandTimeout = 3600;
                    cmdMerge.ExecuteNonQuery();
                }
                endMerge = DateTime.UtcNow;

                // Truncate the Temp Table 
                using (SqlCommand cmdTruncate = new SqlCommand(string.Format("TRUNCATE TABLE Document{0}", objectSuffix), conn))
                {
                    cmdTruncate.ExecuteNonQuery();
                }
            }

            Console.WriteLine(string.Format("BCP Time: {0}", endBCP.Subtract(startBCP).TotalMilliseconds.ToString()));
            Console.WriteLine(string.Format("Merge Time: {0}", endMerge.Subtract(startMerge).TotalMilliseconds.ToString()));
        }

        private static DateTime GetLastSynch()
        {
 
            DateTime lastSynch = DateTime.UtcNow;
            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("ShardAggregation")))
            {
                conn.Open();
                using (SqlCommand cmdReader = new SqlCommand("SELECT LastSynch FROM LastSynch WHERE ApplicationName = @ApplicationName", conn))
                {
                    cmdReader.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName);
                    using (SqlDataReader reader = cmdReader.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            lastSynch = reader.GetDateTime(0);
                        }
                        else
                        {
                            using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO LastSynch(ApplicationName, LastSynch) VALUES (@ApplicationName, @LastSynch)", conn))
                            {
                                cmdInsert.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName);
                                cmdInsert.Parameters.AddWithValue("@LastSynch", lastSynch);
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            return lastSynch;
        }

        private static void UpdateLastSynch(DateTime lastSynch)
        {
 
            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("ShardAggregation")))
            {
                conn.Open();
                using (SqlCommand updateCmd = new SqlCommand("UPDATE LastSynch SET LastSynch = @LastSynch WHERE ApplicationName = @ApplicationName", conn))
                {
                    updateCmd.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName);
                    updateCmd.Parameters.AddWithValue("@LastSynch", lastSynch);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
