using System;
using System.Collections.Generic;
using System.IO;
using FileWatch.Handlers;
using FileWatch.Utils;

namespace FileWatch.Controllers
{
    public interface IFileWatchController
    {
        void Run(IList<string> args);
    }

    public class FileWatchController : IFileWatchController
    {
        private IFileWatchHandler FileWatchHandler { get; set; }
        private IFileMonitorRunner FileMonitorRunner { get; set; }

        public FileWatchController(IFileWatchHandler fileWatchHandler, IFileMonitorRunner fileMonitorRunner)
        {
            FileWatchHandler = fileWatchHandler;
            FileMonitorRunner = fileMonitorRunner;
        }

        public void Run(IList<string> args)
        {
            AssertValidArgs(args);
            this.LogInfo("Starting FileWatch Monitoring.");
            FileMonitorRunner.Created += FileWatchHandler.Created;
            FileMonitorRunner.Modified += FileWatchHandler.Modified;
            FileMonitorRunner.Deleted += FileWatchHandler.Deleted;
            FileMonitorRunner.Start(args[0], args[1], 10000);
            this.LogInfo("Press any key to exit...");
        }

        private void AssertValidArgs(IList<string> args)
        {
            if(args.Count != 2)
                throw new ArgumentException("Invalid number of args, should be 2.");
            if (!Directory.Exists(args[0]))
                throw new ArgumentException("First argument must be a valid and accessible directory.");
        }
    }
}
