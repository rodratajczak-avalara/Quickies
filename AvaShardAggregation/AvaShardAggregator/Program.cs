using System;

namespace AvaShardAggregator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool saveLogFile = true;
            if (args.Length > 0)
            {
                bool.TryParse(args[0], out saveLogFile);
            }
            AggregateShardService processData =  new AggregateShardService(saveLogFile);
            processData.ProcessAvaTaxAccountTables();
        }
    }
}
