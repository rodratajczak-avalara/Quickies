using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace AvaShardReplication
{
    public partial class ReplicateShardService : ServiceBase
    {
        public ReplicateShardService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DateTime lastSynch = GetLastSynch();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Source"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Document WHERE ModifiedDate >= @LastCheckTime", conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", lastSynch );
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        PerformBulkCopy(reader);
                    }
                }
            }
        }

        protected override void OnStop()
        {
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
}
