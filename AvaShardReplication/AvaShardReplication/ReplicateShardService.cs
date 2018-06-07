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

namespace AvaShardReplication
{
    public partial class ReplicateShardService : ServiceBase
    {
        private static string connectionStringSource = "Data Source=.;Initial Catalog=AvaTaxAccount_Shard1;Integrated Security=SSPI;";
        private static string connectionStringDestination = "Data Source=.;Initial Catalog=AvaTaxAccount_Shard1_Repl;Integrated Security=SSPI;";

        public ReplicateShardService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (SqlConnection conn = new SqlConnection(connectionStringSource))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Document WHERE ModifiedDate >= @LastCheckTime", conn))
                {
                    cmd.Parameters.AddWithValue("@LastCheckTime", );
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        performBulkCopy(reader);
                    }
                }
            }


        }

        protected override void OnStop()
        {
        }


        private static void performBulkCopy(SqlDataReader r)
        {
            using (SqlConnection conn = new SqlConnection(connectionStringDestination))
            {
                conn.Open();
                using (SqlBulkCopy cpy = new SqlBulkCopy(conn))
                {
                    cpy.DestinationTableName = "Document_Temp_Shard1";
                    cpy.WriteToServer(r);
                }
            }
        }
    }
}
