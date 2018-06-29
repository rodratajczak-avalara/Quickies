﻿
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private static long _bcpBatchId;
        private static int _aggregationTableId;
        private static bool _enableLogging;


        public AggregateShardService(bool EnableLogging) 
        {
            _enableLogging = EnableLogging;
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


            Stopwatch totalProcessing = new Stopwatch();
            totalProcessing.Start();
            // Process AvaTaxAccount Tables with ModifiedDate on Table
            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdTables = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdTables.CommandTimeout = 30;
                    cmdTables.CommandType = CommandType.Text;
                    cmdTables.CommandText = @"SELECT AggregationTableId, TableName, ParentTableId, FullTable, ModifiedDateExists, RemoveDuplicate
                                                            FROM AggregationTable
                                                            WHERE Enabled = 1
                                                            AND FullTable = 0
                                                            ORDER BY ExecutionGroup, ParentTableId";
                    if (cmdTables.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdTables.Connection.Open();
                    }

                    using (DbDataReader readerTables = cmdTables.ExecuteReader())
                    {
                        bool tableSuccessful = true;
                        while (readerTables.Read())
                        {
                            _aggregationTableId = (int)readerTables["AggregationTableId"];
                            tableSuccessful = ProcessModifiedData(lastSynch, startTime, readerTables["TableName"].ToString(), Boolean.Parse(readerTables["RemoveDuplicate"].ToString()), Boolean.Parse(readerTables["ModifiedDateExists"].ToString()));
                            if (!tableSuccessful)
                            {
                                LogToConsole(string.Format("Unable to process Bcp Merge for table {0}", readerTables["TableName"].ToString()));
                                batchSuccessful = false;
                            }
                        }
                    }

                    cmdTables.Connection.Close();
                }
            }
            totalProcessing.Stop();

            if (batchSuccessful)
            {
                UpdateLastSynch(startTime);
            }

            // Log Total Processing Time
            LogToConsole(string.Format("Total Processing Time: {0}", totalProcessing.ElapsedMilliseconds.ToString()));
            LogBatchProcess(_bcpBatchId, 0, 1, totalProcessing.ElapsedMilliseconds);

            if (_enableLogging)
            {
                Console.ReadKey();
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
            using (SourceContext sContext = new SourceContext())
            {
                using (DbCommand cmdModified = sContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdModified.Parameters.Add(new SqlParameter("@LastCheckTime", StartSynch));
                    cmdModified.Parameters.Add(new SqlParameter("@CurrentCheckTime", EndSynch));
                    cmdModified.CommandTimeout = 60;
                    cmdModified.CommandType = CommandType.Text;
                    cmdModified.CommandText = GetSelectSQL(TableName, ModifiedDateExists); 
                    if (cmdModified.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdModified.Connection.Open();
                    }

                    Stopwatch modifiedTime = new Stopwatch();

                    try
                    {
                        LogToConsole(string.Format("{0} Query Changed Records Started", TableName));
                        modifiedTime.Start();
                        using (DbDataReader reader = cmdModified.ExecuteReader())
                        {
                            modifiedTime.Stop();
                            LogToConsole(string.Format("{0} Query Changed Records Time: {1}", TableName, modifiedTime.ElapsedMilliseconds.ToString()));
                            LogBatchProcess(_bcpBatchId, _aggregationTableId, 2, modifiedTime.ElapsedMilliseconds);

                            PerformBulkCopy(reader, TableName, RemoveDuplicate);
                        }
                        modifiedTime.Reset();
                    }
                    catch (Exception ex)
                    {
                        successful = false;
                        LogToConsole(string.Format("Error occurred processing table {0}:  [{1}]", TableName, ex.Message));
                        LogBatchError(_bcpBatchId, _aggregationTableId, ex.Message);
                    }

                    cmdModified.Connection.Close();
                    modifiedTime = null;
                }
            }

            return successful;
        }

        /// <summary>
        /// Processes steps of aggregation per table (BCP, Delete Dups, Merge, truncate temp)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="TableName"></param>
        /// <param name="RemoveDuplicate"></param>
        private static void PerformBulkCopy(DbDataReader r, string TableName, Boolean RemoveDuplicate)
        {
            //Initialize
            Stopwatch processTime = new Stopwatch();
            TruncateTable(TableName);

            // Process
            #region 1 - Bulk copy the data from the source
            using (DestinationContext dContext = new DestinationContext())
            {
                LogToConsole(string.Format("{0} Bulk Copy Started", TableName));
                processTime.Start();

                if (dContext.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    dContext.Database.GetDbConnection().Open();
                }

                using (var conn = dContext.Database.GetDbConnection() as SqlConnection)
                {
                    using (var cpy = new SqlBulkCopy(conn))
                    {
                        cpy.BulkCopyTimeout = 3600;
                        cpy.DestinationTableName = string.Format("{0}{1}", TableName, _objectSuffix);
                        cpy.BatchSize = int.Parse(_config.GetSection("BcpBatchSize").Value);
                        cpy.WriteToServer(r);
                    }
                }

                processTime.Stop();
                LogToConsole(string.Format("{0} BCP Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                LogBatchProcess(_bcpBatchId, _aggregationTableId, 3, processTime.ElapsedMilliseconds);
                processTime.Reset();
            }
            #endregion

            using (DestinationContext dContext = new DestinationContext())
            {
                #region 2 - Delete Duplicate Natural Keys
                if (RemoveDuplicate)
                {
                    LogToConsole(string.Format("{0} Duplicate Delete Started", TableName));
                    processTime.Start();
                    using (DbCommand cmdDuplicateDelete = dContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmdDuplicateDelete.CommandTimeout = 3600;
                        cmdDuplicateDelete.CommandType = CommandType.StoredProcedure;
                        cmdDuplicateDelete.CommandText = string.Format("sp_Delete_Duplicate_{0}_with{1}", TableName, _objectSuffix);
                        if (cmdDuplicateDelete.Connection.State != System.Data.ConnectionState.Open)
                        {
                                cmdDuplicateDelete.Connection.Open();
                        }

                        cmdDuplicateDelete.ExecuteNonQuery();

                        cmdDuplicateDelete.Connection.Close();
                    }
                    processTime.Stop();
                    LogToConsole(string.Format("{0} Duplicate Delete Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                    LogBatchProcess(_bcpBatchId, _aggregationTableId, 4, processTime.ElapsedMilliseconds);
                    processTime.Reset();
                }
                #endregion

                #region 3 - Perform Merge of Data
                LogToConsole(string.Format("{0} Merge Started", TableName));
                processTime.Start();
                using (DbCommand cmdMerge = dContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdMerge.CommandTimeout = 3600;
                    cmdMerge.CommandType = CommandType.StoredProcedure;
                    cmdMerge.CommandText = string.Format("sp_Merge_{0}_From{1}", TableName, _objectSuffix);
                    if (cmdMerge.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdMerge.Connection.Open();
                    }

                    cmdMerge.ExecuteNonQuery();

                    cmdMerge.Connection.Close();
                }
                processTime.Stop();
                LogToConsole(string.Format("{0} Merge Time: {1}", TableName, processTime.ElapsedMilliseconds.ToString()));
                LogBatchProcess(_bcpBatchId, _aggregationTableId, 5, processTime.ElapsedMilliseconds);
                processTime.Reset();
                #endregion
            }

            // Breakdown
            TruncateTable(TableName);
            processTime = null;
        }

        #region Miscellaneous helper methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static DateTime GetLastSynch()
        {
 
            DateTime lastSynch = DateTime.UtcNow;

            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdReader = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdReader.Parameters.Add(new SqlParameter("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix));
                    cmdReader.CommandTimeout = 60;
                    cmdReader.CommandType = CommandType.Text;
                    cmdReader.CommandText = "SELECT LastSynch FROM LastSynch WHERE ApplicationName = @ApplicationName";
                    if (cmdReader.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdReader.Connection.Open();
                    }

                    using (DbDataReader reader = cmdReader.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            lastSynch = reader.GetDateTime(0);
                        }
                        else
                        {
                            using (DbCommand cmdInsert = saContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmdInsert.Parameters.Add(new SqlParameter("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix));
                                cmdInsert.Parameters.Add(new SqlParameter("@LastSynch", lastSynch));
                                cmdInsert.CommandTimeout = 60;
                                cmdInsert.CommandType = CommandType.Text;
                                cmdInsert.CommandText = "INSERT INTO LastSynch(ApplicationName, LastSynch) VALUES (@ApplicationName, @LastSynch)";
                                if (cmdInsert.Connection.State != System.Data.ConnectionState.Open)
                                {
                                    cmdInsert.Connection.Open();
                                }

                                cmdInsert.ExecuteNonQuery();

                                cmdInsert.Connection.Close();
                            }
                        }
                    }
                }
            }

            return lastSynch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastSynch"></param>
        private static void UpdateLastSynch(DateTime lastSynch)
        {

            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdUpdate = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix));
                    cmdUpdate.Parameters.Add(new SqlParameter("@LastSynch", lastSynch));
                    cmdUpdate.CommandTimeout = 60;
                    cmdUpdate.CommandType = CommandType.Text;
                    cmdUpdate.CommandText = "UPDATE LastSynch SET LastSynch = @LastSynch WHERE ApplicationName = @ApplicationName";
                    if (cmdUpdate.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdUpdate.Connection.Open();
                    }

                    cmdUpdate.ExecuteNonQuery();

                    cmdUpdate.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private static long LogBcpBatch(DateTime startTime)
        {
            long bcpBatchId = 0;

            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdInsert = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdInsert.Parameters.Add(new SqlParameter("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName + _objectSuffix));
                    cmdInsert.Parameters.Add(new SqlParameter("@BatchStartTime", startTime));
                    cmdInsert.CommandTimeout = 60;
                    cmdInsert.CommandType = CommandType.Text;
                    cmdInsert.CommandText = "INSERT BCPBatch (ApplicationName, BatchStartTime) output INSERTED.BCPBatchId VALUES(@ApplicationName, @BatchStartTime)";
                    if (cmdInsert.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdInsert.Connection.Open();
                    }

                    bcpBatchId = (long)cmdInsert.ExecuteScalar();

                    cmdInsert.Connection.Close();
                }
            }

            return bcpBatchId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BCPBatchId"></param>
        /// <param name="AggregationTableId"></param>
        /// <param name="BatchProcessId"></param>
        /// <param name="ElapsedTime"></param>
        private static void LogBatchProcess(long BCPBatchId, int AggregationTableId, int BatchProcessId,  long ElapsedTime)
        {
            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdInsert = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdInsert.Parameters.Add(new SqlParameter("@AggregationTableId", AggregationTableId));
                    cmdInsert.Parameters.Add(new SqlParameter("@BCPBatchId", BCPBatchId));
                    cmdInsert.Parameters.Add(new SqlParameter("@BatchProcessId", BatchProcessId));
                    cmdInsert.Parameters.Add(new SqlParameter("@BatchProcessTime", ElapsedTime));
                    cmdInsert.CommandTimeout = 60;
                    cmdInsert.CommandType = CommandType.Text;
                    cmdInsert.CommandText = "INSERT BCPBatchLog (BCPBatchId, AggregationTableId, BatchProcessId, BatchProcessTime) VALUES(@BCPBatchId, @AggregationTableId, @BatchProcessId, @BatchProcessTime)";
                    if (cmdInsert.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdInsert.Connection.Open();
                    }

                    cmdInsert.ExecuteNonQuery();

                    cmdInsert.Connection.Close();
                }
            }
        }


        /// <summary>
        /// save an error associated with a batch for future reference
        /// </summary>
        /// <param name="BCPBatchId"></param>
        /// <param name="AggregationTableId"></param>
        /// <param name="ErrorMessage"></param>
        private static void LogBatchError(long BCPBatchId, int AggregationTableId, string ErrorMessage)
        {
            using (ShardAggregationContext saContext = new ShardAggregationContext())
            {
                using (DbCommand cmdInsert = saContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdInsert.Parameters.Add(new SqlParameter("@BCPBatchId", BCPBatchId));
                    cmdInsert.Parameters.Add(new SqlParameter("@AggregationTableId", AggregationTableId));
                    cmdInsert.Parameters.Add(new SqlParameter("@errormessage", ErrorMessage));
                    cmdInsert.CommandTimeout = 60;
                    cmdInsert.CommandType = CommandType.Text;
                    cmdInsert.CommandText = "INSERT INTO dbo.BCPBatchError(BCPBatchId, AggregationTableId, ErrorMessage) VALUES(@bcpbatchid, @aggregationtableid, @errormessage)";
                    if (cmdInsert.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdInsert.Connection.Open();
                    }

                    cmdInsert.ExecuteNonQuery();

                    cmdInsert.Connection.Close();
                }
            }
        }
        
        private static void LogToConsole(string Message)
        {
            if (_enableLogging)
            {
                Console.WriteLine(Message);
            }
        }

        private static void TruncateTable(string TableName)
        {
            using (DestinationContext dContext = new DestinationContext())
            {

                using (DbCommand cmdTruncate = dContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdTruncate.CommandTimeout = 300;
                    cmdTruncate.CommandType = CommandType.Text;
                    cmdTruncate.CommandText = string.Format("TRUNCATE TABLE [{0}{1}]", TableName, _objectSuffix);
                    if (cmdTruncate.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmdTruncate.Connection.Open();
                    }

                    cmdTruncate.ExecuteNonQuery();

                    cmdTruncate.Connection.Close();
                }
            }
        }
        /// <summary>
        /// obtain the SQL needed 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ModifiedDateExists"></param>
        /// <returns></returns>
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
                    case "AccountFeatureList":
                        cmdSQL = @" SELECT afl.*
                                        FROM Account a WITH (NOLOCK)
                                        INNER JOIN AccountFeatureList afl WITH (NOLOCK, FORCESEEK)
                                        ON a.AccountId = afl.AccountId
                                        WHERE a.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "CombinedHSTConfig":
                        cmdSQL = @" SELECT cc.*
                                        FROM Account a WITH (NOLOCK)
                                        INNER JOIN CombinedHSTConfig cc WITH (NOLOCK, FORCESEEK)
                                        ON a.AccountId = cc.AccountId
                                        WHERE a.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "CompanyLocationSetting":
                        cmdSQL = @" SELECT cls.*
                                        FROM CompanyLocation cl WITH (NOLOCK)
                                        INNER JOIN CompanyLocationSetting cls WITH (NOLOCK, FORCESEEK)
                                        ON cl.CompanyLocationId = cls.CompanyLocationId
                                        WHERE cl.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "CompanyRPSSeries":
                        cmdSQL = @" SELECT crs.*
                                        FROM Company c WITH (NOLOCK)
                                        INNER JOIN CompanyRPSSeries crs WITH (NOLOCK, FORCESEEK)
                                        ON c.CompanyId = crs.CompanyId
                                        WHERE c.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "CompanySetting":
                        cmdSQL = @" SELECT cs.*
                                        FROM Company c WITH (NOLOCK)
                                        INNER JOIN CompanySetting cs WITH (NOLOCK, FORCESEEK)
                                        ON c.CompanyId = cs.CompanyId
                                        WHERE c.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "CompanyDistanceThreshold":
                        cmdSQL = @" SELECT cdt.*
                                        FROM Company c WITH (NOLOCK)
                                        INNER JOIN CompanyDistanceThreshold cdt WITH (NOLOCK, FORCESEEK)
                                        ON c.CompanyId = cdt.CompanyId
                                        WHERE c.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentAddress":
                        cmdSQL = @"	SELECT da.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentAddress da WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = da.DocumentId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentParameterBag":
                        cmdSQL = @"	SELECT dpb.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentParameterBag dpb WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dpb.DocumentId 
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentProperty":
                        cmdSQL = @"	SELECT dp.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentProperty dp WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dp.DocumentId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentLine":
                        cmdSQL = @"	SELECT dl.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentLine dl WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dl.DocumentId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentLineParameterBag":
                        cmdSQL = @"	SELECT dlpb.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentLine dl WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dl.DocumentId
                                        INNER JOIN DocumentLineParameterBag dlpb  WITH (NOLOCK, FORCESEEK)
                                        ON dl.DocumentLineId = dlpb.DocumentLineId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentLineProperty":
                        cmdSQL = @"	SELECT dlp.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentLine dl WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dl.DocumentId
                                        INNER JOIN DocumentLineProperty dlp  WITH (NOLOCK, FORCESEEK)
                                        ON dl.DocumentLineId = dlp.DocumentLineId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentLineDetail":
                        cmdSQL = @"	SELECT dld.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentLine dl WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dl.DocumentId
                                        INNER JOIN DocumentLineDetail dld WITH (NOLOCK, FORCESEEK)
                                        ON dl.DocumentLineId = dld.DocumentLineId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "DocumentLineDetailProperty":
                        cmdSQL = @"	SELECT dldp.*
	                                    FROM Document d WITH (NOLOCK)
	                                    INNER JOIN DocumentLine dl WITH (NOLOCK, FORCESEEK)
	                                    ON d.DocumentId = dl.DocumentId
                                        INNER JOIN DocumentLineDetail dld WITH (NOLOCK, FORCESEEK)
                                        ON dl.DocumentLineId = dld.DocumentLineId
                                        INNER JOIN DocumentLineDetailProperty dldp  WITH (NOLOCK, forceseek)
                                        ON dld.DocumentLineDetailId = dldp.DocumentLineDetailId
	                                    WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "ExemptCertDetail":
                        cmdSQL = @" SELECT ecd.*
                                        FROM ExemptCert ec WITH (NOLOCK)
                                        INNER JOIN ExemptCertDetail ecd WITH (NOLOCK, FORCESEEK)
                                        ON ec.ExemptCertId = ecd.ExemptCertId
                                        WHERE ec.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "MaxLine":
                        cmdSQL = @" SELECT ml.*
                                        FROM Account a WITH (NOLOCK)
                                        INNER JOIN MaxLine ml WITH (NOLOCK, FORCESEEK)
                                        ON a.AccountId = ml.AccountId
                                        WHERE a.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                    case "SubscriptionSetting":
                        cmdSQL = @" SELECT ss.*
                                        FROM Subscription s WITH (NOLOCK)
                                        INNER JOIN SubscriptionSetting ss WITH (NOLOCK, FORCESEEK)
                                        ON s.SubscriptionId = ss.SubscriptionSettingId
                                        WHERE s.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime";
                        break;
                }
            }

            return cmdSQL;
        }
        #endregion
    }
}
