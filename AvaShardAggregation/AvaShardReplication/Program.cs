using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace AvaShardReplication
{
    static class Program
    {
        #region [ Local Variables ]

        private static Scheduler _scheduler;
        private static ControlEventHandler _dontGarbageCollectMePlease;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            var host = WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();

            host.RunAsService();
            if (Debugger.IsAttached || args.Contains("--debug"))
            {
                //we need to keep a reference to the delegate or it will get GC'd and that's bad
                _dontGarbageCollectMePlease = new ControlEventHandler(OnControlEvent);
                SetConsoleCtrlHandler(_dontGarbageCollectMePlease, true);

                _scheduler = new Scheduler();
                _scheduler.ShutdownEvent += new EventHandler(HandleShutdown);
                _scheduler.Go(); // busy loop

                _scheduler.Dispose();
            }
            else
            {
                System.ServiceProcess.ServiceBase.Run(new ReplicateShardService());
            }
        }

    }
}
