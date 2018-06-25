
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
        private static long _bcpBatchId;
        private static int _aggregationTableId;


        public AggregateShardService(bool saveLogFile) 
        {
            _saveLogToFile = saveLogFile;
        }

        /// <summary>
        /// Main processing module to obtain tables to aggregate and loop through the aggregation process
        /// </summary>
        public void ProcessAvaTaxAccountTables()
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime lastSynch = GetLastSynch();
            if (startTime.Subtract(lastSynch).TotalHours > double.Parse(_config.GetSection("MaxCopyHours").Value))
            {
                startTime = lastSynch.AddHours(double.Parse(_config.GetSection("MaxCopyHours").Value));
            }
            _bcpBatchId = LogBcpBatch(startTime);
            bool batchSuccessful = true;

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
                        using (SqlCommand cmdTables = new SqlCommand(@"SELECT AggregationTableId, TableName, ParentTableId, FullTable, ModifiedDateExists, RemoveDuplicate
                                                                    FROM AggregationTable
                                                                    WHERE Enabled = 1
                                                                    AND FullTable = 0
                                                                    ORDER BY ExecutionGroup, ParentTableId", conn))
                        {
                            using (SqlDataReader readerTables = cmdTables.ExecuteReader())
                            {
                                bool tableSuccessful = true;
                                while (readerTables.Read())
                                {
                                    _aggregationTableId = (int)readerTables["AggregationTableId"];
                                    tableSuccessful = ProcessModifiedData(lastSynch, startTime, readerTables["TableName"].ToString(), Boolean.Parse(readerTables["RemoveDuplicate"].ToString()), Boolean.Parse(readerTables["ModifiedDateExists"].ToString()));
                                    if (!tableSuccessful)
                                    {
                                        Console.WriteLine(string.Format("Unable to process Bcp Merge for table {0}", readerTables["TableName"].ToString()));
                                        batchSuccessful = false;
                                    }
                                }
                            }
                        }
                    }
                    totalProcessing.Stop();

                    if (batchSuccessful)
                    {
                        UpdateLastSynch(startTime);
                    }

                    // Log Total Processing Time
                    Console.WriteLine(string.Format("Total Processing Time: {0}", totalProcessing.ElapsedMilliseconds.ToString()));
                    LogBatchProcess(_bcpBatchId, 0, 1, totalProcessing.ElapsedMilliseconds);

                    if (_saveLogToFile)
                    {
                        Console.SetOut(oldOut);
                    }
                    else
                    {
                        //Console.ReadKey();
                    }
                }
            }
           
        }

        /// <summary>
        /// Method to get modified records for a table and kick off aggregations for that table
        /// </summary>
        /// <param name="StartSynch"></param>
        /// <param name="EndSynch"></param>
        /// <param name="TableName"></param>
        /// <param name="RemoveDuplicate"></param>
        /// <param name="ModifiedDateExists"></param>
        /// <returns></returns>
        private static bool ProcessModifiedData(DateTime StartSynch, DateTime EndSynch, string TableName, Boolean RemoveDuplicate, Boolean ModifiedDateExists)
        {
            bool successful = true;
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("Source")))
            {
                Stopwatch modifiedTime = new Stopwatch();

                conn.Open();

                string cmdSQL = GetSelectSQL(TableName, ModifiedDateExists);
                using (SqlCommand cmd = new SqlCommand(cmdSQL, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@LastCheckTime", StartSynch);
                        cmd.Parameters.AddWithValue("@CurrentCheckTime", EndSynch);
                        cmd.CommandTimeout = 600;
                        Console.WriteLine(string.Format("{0} Query Changed Records Started", TableName));
                        modifiedTime.Start();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            modifiedTime.Stop();
                            Console.WriteLine(string.Format("{0} Query Changed Records Time: {1}", TableName, modifiedTime.ElapsedMilliseconds.ToString()));
                            LogBatchProcess(_bcpBatchId, _aggregationTableId, 2, modifiedTime.ElapsedMilliseconds);

                            PerformBulkCopy(reader, TableName, RemoveDuplicate);
                        }
                        modifiedTime.Reset();
                    }
                    catch (Exception ex)
                    {
                        successful = false;
                        Console.WriteLine(string.Format("Error occurred processing table {0}:  [{1}]", TableName, ex.Message));
                    }
                }
                modifiedTime = null;
            }

            return successful;
        }

        /// <summary>
        /// Processes steps of aggregation per table (BCP, Delete Dups, Merge, truncate temp)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="TableName"></param>
        /// <param name="RemoveDuplicate"></param>
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
                LogBatchProcess(_bcpBatchId, _aggregationTableId, 3, processTime.ElapsedMilliseconds);
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
                    LogBatchProcess(_bcpBatchId, _aggregationTableId, 4, processTime.ElapsedMilliseconds);
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
                LogBatchProcess(_bcpBatchId, _aggregationTableId, 5, processTime.ElapsedMilliseconds);
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
                    cmdReader.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix);
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
                                cmdInsert.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix);
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
                    updateCmd.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix);
                    updateCmd.Parameters.AddWithValue("@LastSynch", lastSynch);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        private static long LogBcpBatch(DateTime startTime)
        {
            long bcpBatchId = 0;
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ShardAggregation")))
            {
                conn.Open();
                using (SqlCommand insertCmd = new SqlCommand("INSERT BCPBatch (ApplicationName, BatchStartTime) output INSERTED.BCPBatchId VALUES(@ApplicationName, @BatchStartTime)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix);
                    insertCmd.Parameters.AddWithValue("@BatchStartTime", startTime);
                    bcpBatchId = (long)insertCmd.ExecuteScalar();
                }
            }

            return bcpBatchId;
        }

        private static void LogBatchProcess(long BCPBatchId, int AggregationTableId, int BatchProcessId,  long ElapsedTime)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("ShardAggregation")))
            {
                conn.Open();
                using (SqlCommand insertCmd = new SqlCommand("INSERT BCPBatchLog (BCPBatchId, AggregationTableId, BatchProcessId, BatchProcessTime) VALUES(@BCPBatchId, @AggregationTableId, @BatchProcessId, @BatchProcessTime)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@AggregationTableId", AggregationTableId);
                    insertCmd.Parameters.AddWithValue("@BCPBatchId", BCPBatchId);
                    insertCmd.Parameters.AddWithValue("@BatchProcessId", BatchProcessId);
                    insertCmd.Parameters.AddWithValue("@BatchProcessTime", ElapsedTime);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetSelectSQL(string TableName, bool ModifiedDateExists)
        {
            string cmdSQL = string.Empty;
            if (ModifiedDateExists)
            {
                cmdSQL = string.Format("SELECT * FROM [{0}] WITH (NOLOCK) WHERE ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime", TableName);
            }
            else
            {
                switch (TableName)
                {
                    case "DocumentAddress":
                        cmdSQL = string.Format(@"	SELECT da.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentAddress da WITH (NOLOCK)
	                                                    ON d.DocumentId = da.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        and da.DocumentAddressId IS NOT NULL", TableName);
                        break;
                    case "DocumentParameterBag":
                        cmdSQL = string.Format(@"	SELECT dpb.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentParameterBag dpb WITH (NOLOCK)
	                                                    ON d.DocumentId = dpb.DocumentId 
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dpb.DocumentParameterBagId IS NOT NULL", TableName);
                        break;
                    case "DocumentProperty":
                        cmdSQL = string.Format(@"	SELECT dp.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentProperty dp WITH (NOLOCK)
	                                                    ON d.DocumentId = dp.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dp.DocumentPropertyId IS NOT NULL", TableName);
                        break;
                    case "DocumentLine":
                        cmdSQL = string.Format(@"	SELECT dl.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentLine dl WITH (NOLOCK)
	                                                    ON d.DocumentId = dl.DocumentId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dl.DocumentLineId IS NOT NULL", TableName);
                        break;
                    case "DocumentLineParameterBag":
                        cmdSQL = string.Format(@"	SELECT dlpb.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentLine dl WITH (NOLOCK)
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineParameterBag dlpb  WITH (NOLOCK)
                                                        ON dl.DocumentLineId = dlpb.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dlpb.DocumentLineParameterBagId IS NOT NULL", TableName);
                        break;
                    case "DocumentLineProperty":
                        cmdSQL = string.Format(@"	SELECT dlp.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentLine dl WITH (NOLOCK)
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineProperty dlp  WITH (NOLOCK)
                                                        ON dl.DocumentLineId = dlp.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dlp.DocumentLinePropertyId IS NOT NULL", TableName);
                        break;
                    case "DocumentLineDetail":
                        cmdSQL = string.Format(@"	SELECT dld.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentLine dl WITH (NOLOCK)
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineDetail dld WITH (NOLOCK)
                                                        ON dl.DocumentLineId = dld.DocumentLineId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dld.DocumentLineDetailId IS NOT NULL", TableName);
                        break;
                    case "DocumentLineDetailProperty":
                        cmdSQL = string.Format(@"	SELECT dldp.*
	                                                    FROM Document d WITH (NOLOCK)
	                                                    LEFT JOIN DocumentLine dl WITH (NOLOCK)
	                                                    ON d.DocumentId = dl.DocumentId
                                                        LEFT JOIN DocumentLineDetail dld WITH (NOLOCK)
                                                        ON dl.DocumentLineId = dld.DocumentLineId
                                                        LEFT JOIN DocumentLineDetailProperty dldp  WITH (NOLOCK)
                                                        ON dld.DocumentLineDetailId = dldp.DocumentLineDetailId
	                                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
                                                        AND dldp.DocumentLineDetailId IS NOT NULL", TableName);
                        break;
                }
            }

            return cmdSQL;
        }
        #endregion
    }
}
