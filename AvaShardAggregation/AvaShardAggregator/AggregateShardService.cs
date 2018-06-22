
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
        private static IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        private static string _objectSuffix = _config.GetSection("ObjectSuffix").Value;
        private static bool _saveLogToFile = false;


        public AggregateShardService(bool saveLogFile) 
        {
            _saveLogToFile = saveLogFile;
        }

        public void ProcessAvaTaxAccountTables()
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime lastSynch = GetLastSynch();

            using (FileStream ostrm = new FileStream(string.Format("./logs/BCPMerge_{0}.log", startTime.ToFileTime().ToString()), FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(ostrm))
                {
                    TextWriter oldOut = Console.Out;

                    // Determine Logging destination
                    if (_saveLogToFile)
                    {
                        Console.SetOut(writer);
                    }

                    Stopwatch totalProcessing = new Stopwatch();
                    totalProcessing.Start();
                    // Process AvaTaxAccount Tables with ModifiedDate on Table
                    using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ShardAggregation")))
                    {
                        conn.Open();
                        using (SqlCommand cmdTables = new SqlCommand(@"SELECT TableName, ParentTableId, FullTable, ModifiedDateExists, RemoveDuplicate
                                                                    FROM AggregationTable
                                                                    WHERE Enabled = 1
                                                                    AND FullTable = 0
                                                                    ORDER BY ExecutionGroup, ParentTableId", conn))
                        {
                            using (SqlDataReader readerTables = cmdTables.ExecuteReader())
                            {
                                while (readerTables.Read())
                                {
                                    ProcessModifiedData(lastSynch, startTime, readerTables["TableName"].ToString(), Boolean.Parse(readerTables["RemoveDuplicate"].ToString()), Boolean.Parse(readerTables["ModifiedDateExists"].ToString()));
                                }
                            }
                        }
                    }
                    totalProcessing.Stop();
                    UpdateLastSynch(startTime);

                    // Log Total Processing Time
                    Console.WriteLine(string.Format("Total Processing Time: {0}", totalProcessing.ElapsedMilliseconds.ToString()));

                    if (_saveLogToFile)
                    {
                        Console.SetOut(oldOut);
                    }
                    else
                    {
                        Console.ReadKey();
                    }
                }
            }
           
        }

        private static void ProcessModifiedData(DateTime StartSynch, DateTime EndSynch, string TableName, Boolean RemoveDuplicate, Boolean ModifiedDateExists)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("Source")))
            {
                Stopwatch modifiedTime = new Stopwatch();

                conn.Open();

                string cmdSQL = string.Empty;
                if (ModifiedDateExists)
                {
                    cmdSQL = string.Format("SELECT * FROM [{0}] WHERE ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                }
                else
                {
                    switch (TableName)
                    {
                        case "DocumentAddress":
                            cmdSQL = string.Format(@"	SELECT da.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentAddress da
	                                                    ON d.DocumentId = da.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentParameterBag":
                            cmdSQL = string.Format(@"	SELECT dpb.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentParameterBag dpb
	                                                    ON d.DocumentId = dpb.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentProperty":
                            cmdSQL = string.Format(@"	SELECT dp.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentProperty dp
	                                                    ON d.DocumentId = dp.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentLine":
                            cmdSQL = string.Format(@"	SELECT dl.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentLine dl
	                                                    ON d.DocumentId = dl.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentLineParameterBag":
                            cmdSQL = string.Format(@"	SELECT dlpb.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentLine dl
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineParameterBag dlpb
                                                        ON dl.DocumentLineId = dlpb.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentLineProperty":
                            cmdSQL = string.Format(@"	SELECT dlp.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentLine dl
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineProperty dlp
                                                        ON dl.DocumentLineId = dlp.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentLineDetail":
                            cmdSQL = string.Format(@"	SELECT dld.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentLine dl
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineDetail dld
                                                        ON dl.DocumentLineId = dld.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                            break;
                        case "DocumentLineDetailProperty":
                            cmdSQL = string.Format(@"	SELECT dldp.*
	                                                    FROM Document d
	                                                    LEFT JOIN DocumentLine dl
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineDetail dld
                                                        ON dl.DocumentLineId = dld.DocumentLineId
                                                        LEFT JOIN DocumentLineDetailProperty dldp
                                                        ON dld.DocumentLineDetailId = dldp.DocumentLineDetailId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
                                    break;
                    }
                }

                using (SqlCommand cmd = new SqlCommand(cmdSQL, conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", StartSynch);
                    cmd.Parameters.AddWithValue("@CurrentCheckTime", EndSynch);
                    Console.WriteLine(string.Format("{0} Query Changed Records Started", TableName));
                    modifiedTime.Start();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        modifiedTime.Stop();
                        Console.WriteLine(string.Format("{0} Query Changed Records Time: {1}", TableName, modifiedTime.ElapsedMilliseconds.ToString()));

                        PerformBulkCopy(reader, TableName, RemoveDuplicate);
                    }
                    modifiedTime.Reset();
                }
                modifiedTime = null;
            }
        }

        private static void PerformBulkCopy(SqlDataReader r, string TableName, Boolean RemoveDuplicate)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("Destination")))
            {
                Stopwatch processTime = new Stopwatch();
                conn.Open();

                #region 1 - Bulk copy the data from the source
                Console.WriteLine(string.Format("{0} Bulk Copy Started", TableName));
                processTime.Start();
                using (SqlBulkCopy cpy = new SqlBulkCopy(conn))
                {
                    cpy.BulkCopyTimeout = 3600;
                    cpy.DestinationTableName = string.Format("{0}{1}", TableName, _objectSuffix);
                    cpy.WriteToServer(r);
                }
                processTime.Stop();
                Console.WriteLine(string.Format("{0} BCP Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                processTime.Reset();
                #endregion

                #region 2 - Delete Duplicate Natural Keys
                if (RemoveDuplicate)
                {
                    Console.WriteLine(string.Format("{0} Duplicate Delete Started", TableName));
                    processTime.Start();
                    using (SqlCommand cmdDuplicateDelete = new SqlCommand(string.Format("sp_Delete_Duplicate_{0}_with{1}", TableName, _objectSuffix), conn))
                    {
                        cmdDuplicateDelete.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdDuplicateDelete.CommandTimeout = 3600;
                        cmdDuplicateDelete.ExecuteNonQuery();
                    }
                    processTime.Stop();
                    Console.WriteLine(string.Format("{0} Duplicate Delete Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                    processTime.Reset();
                }
                #endregion

                #region 3 - Perform Merge of Data
                Console.WriteLine(string.Format("{0} Merge Started", TableName));
                processTime.Start();
                using (SqlCommand cmdMerge = new SqlCommand(string.Format("sp_Merge_{0}_From{1}", TableName, _objectSuffix), conn))
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
                using (SqlCommand cmdTruncate = new SqlCommand(string.Format("TRUNCATE TABLE [{0}{1}]", TableName, _objectSuffix), conn))
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
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ShardAggregation")))
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
 
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ShardAggregation")))
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
