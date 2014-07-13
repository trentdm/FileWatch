using System;
using FileWatch.Controllers;
using FileWatch.Utils;

namespace FileWatch
{
    public class FileWatch
    {
        static void Main(string[] args)
        {
            var fileWatch = new FileWatch();
            try
            {
                fileWatch.Run(args);
            }
            catch (Exception ex)
            {
                fileWatch.LogFatal(ex);
                Environment.ExitCode = -1;
            }
            finally
            {
                Console.ReadLine();
            }
        }

        public void Run(string[] args)
        {
            IoCContainer.Init();
            var controller = IoCContainer.Resolve<IFileWatchController>();
            controller.Run(args);
        }
    }
}
