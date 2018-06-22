
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Diagnostics;

namespace AvaShardAggregator
{
    internal class AggregateShardService
    {
        private static IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        private static string objectSuffix = config.GetSection("ObjectSuffix").Value;
        private static bool saveLogToFile = false;


        public AggregateShardService(bool saveLogFile) 
        {

        }

        public static void ProcessAvaTaxAccountTables()
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime lastSynch = GetLastSynch();
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            // Determine Logging destination
            if (saveLogToFile)
            {
                try
                {
                    ostrm = new FileStream(string.Format("./logs/BCPMerge_{0}.log", startTime.ToFileTime().ToString()), FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter(ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open BCPMerge.log for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.SetOut(writer);
            }

            Stopwatch totalProcessing = new Stopwatch();
            totalProcessing.Start();
            // Process AvaTaxAccount Tables with ModifiedDate on Table
            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("ShardAggregation")))
            {
                conn.Open();
                using (SqlCommand cmdTables = new SqlCommand(@"SELECT TableName, ParentTableId, FullTable, ModifiedDateExists
                                                                FROM AggregationTable
                                                                WHERE Enabled = 1
                                                                AND FullTable = 0
                                                                AND ModifiedDateExists = 1
                                                                ORDER BY ExecutionGroup, ParentTableId", conn))
                {
                    using (SqlDataReader readerTables = cmdTables.ExecuteReader())
                    {
                        while (readerTables.Read())
                        {
                            ProcessModifiedData(lastSynch, startTime, readerTables["TableName"].ToString());
                        }
                    }
                }
            }
            totalProcessing.Stop();
            UpdateLastSynch(startTime);

            // Log Total Processing Time
            Console.WriteLine(string.Format("Total Processing Time: {0}", endTime.Subtract(startTime).TotalMilliseconds).ToString());

            if (saveLogToFile)
            {
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
            }
            //Console.ReadKey();
        }

        private static void ProcessModifiedData(DateTime StartSynch, DateTime EndSynch, string TableName)
        {
            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("Source")))
            {
                Stopwatch modifiedTime = new Stopwatch();

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM [{0}] WHERE ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName), conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", StartSynch);
                    cmd.Parameters.AddWithValue("@CurrentCheckTime", EndSynch);
                    Console.WriteLine(string.Format("{0} Query Changed Records Started", TableName));
                    modifiedTime.Start();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        modifiedTime.Stop();
                        Console.WriteLine(string.Format("{0} Query Changed Records Time: {1}", TableName, modifiedTime.ElapsedMilliseconds.ToString()));

                        PerformBulkCopy(reader, TableName);
                    }
                    modifiedTime.Reset();
                }
                modifiedTime = null;
            }
        }

        private static void PerformBulkCopy(SqlDataReader r, string TableName)
        {
            using (SqlConnection conn = new SqlConnection(config.GetConnectionString("Destination")))
            {
                Stopwatch processTime = new Stopwatch();
                conn.Open();

                #region 1 - Bulk copy the data from the source
                Console.WriteLine(string.Format("{0} Bulk Copy Started", TableName));
                processTime.Start();
                using (SqlBulkCopy cpy = new SqlBulkCopy(conn))
                {
                    cpy.BulkCopyTimeout = 3600;
                    cpy.DestinationTableName = string.Format("{0}{1}", TableName, objectSuffix);
                    cpy.WriteToServer(r);
                }
                processTime.Stop();
                Console.WriteLine(string.Format("{0} BCP Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                processTime.Reset();
                #endregion

                #region 2 - Delete Duplicate Natural Keys
                Console.WriteLine(string.Format("{0} Duplicate Delete Started", TableName));
                processTime.Start();
                using (SqlCommand cmdDuplicateDelete = new SqlCommand(string.Format("sp_Delete_Duplicate_{0}_with{1}", TableName, objectSuffix), conn))
                {
                    cmdDuplicateDelete.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdDuplicateDelete.CommandTimeout = 3600;
                    cmdDuplicateDelete.ExecuteNonQuery();
                }
                processTime.Stop();
                Console.WriteLine(string.Format("{0} Duplicate Delete Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                processTime.Reset();
                #endregion

                #region 3 - Perform Merge of Data
                Console.WriteLine(string.Format("{0} Merge Started", TableName));
                processTime.Start();
                using (SqlCommand cmdMerge = new SqlCommand(string.Format("sp_Merge_{0}_From{1}", TableName, objectSuffix), conn))
                {
                    cmdMerge.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdMerge.CommandTimeout = 3600;
                    cmdMerge.ExecuteNonQuery();
                }

                processTime.Stop();
                Console.WriteLine(string.Format("{0} Merge Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                processTime.Reset();
                #endregion

                #region 4 - Truncate the Temp Table 
                using (SqlCommand cmdTruncate = new SqlCommand(string.Format("TRUNCATE TABLE [{0}{1}]", TableName, objectSuffix), conn))
                {
                    cmdTruncate.ExecuteNonQuery();
                }
                #endregion

                processTime = null;
            }
        }

        #region Synch Time helper methods
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
        #endregion
    }
}
