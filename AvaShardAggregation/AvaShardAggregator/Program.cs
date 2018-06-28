using System;
using System.Threading.Tasks;

namespace AvaShardAggregator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool enableLogging = false;
            if (args.Length>0)
            {
                enableLogging = bool.Parse(args[0]);
            }
            AggregateShardService processData =  new AggregateShardService(enableLogging);
            processData.ProcessAvaTaxAccountTables();
        }
    }
}
