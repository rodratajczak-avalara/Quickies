using System;
using System.Data.SqlClient;
using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.Hosting;

namespace AvaShardAggregation
{
    internal class AggregateShardService : WebHostService
    {
        public AggregateShardService(IWebHost host) : base(host)
        {

        }

        public void Configure()
        {

        }

        protected override void OnStarting(string[] args)
        {
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            DateTime lastSynch = GetLastSynch();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Source"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Document WHERE ModifiedDate >= @LastCheckTime", conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", lastSynch);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        PerformBulkCopy(reader);
                    }
                }
            }

        }

        protected override void OnStopping()
        {
            base.OnStopping();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        private static void PerformBulkCopy(SqlDataReader r)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Destination"].ConnectionString))
            {
                conn.Open();
                using (SqlBulkCopy cpy = new SqlBulkCopy(conn))
                {
                    cpy.DestinationTableName = "Document_Temp_Shard1";
                    cpy.WriteToServer(r);
                }
            }
        }

        private static DateTime GetLastSynch()
        {
            DateTime lastSynch = DateTime.UtcNow;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ShardAggregation"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT LastSynch FROM LastSynch WHERE ApplicationName >= @ApplicationName", conn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationName", System.AppDomain.CurrentDomain.FriendlyName);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            lastSynch = reader.GetFieldValue<DateTime>(0);
                        }
                    }
                }
            }

            return lastSynch;
        }
    }


    public static class AggregateShardServiceExtensions
    {
        public static void AggregateShardService(this IWebHost host)
        {
            var webHostService = new AggregateShardService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
