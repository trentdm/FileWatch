using System.Threading;
using System.Threading.Tasks;
using FileWatch.Handlers;
using Moq;
using NUnit.Framework;

namespace FileWatch.Test.Handlers
{
    [TestFixture]
    public class FileMonitorRunnerTests
    {
        private IFileMonitorRunner FileMonitorRunner { get; set; }
        private Mock<IFileMonitor> FileMonitor { get; set; }

        [SetUp]
        public void Setup()
        {
            FileMonitor = new Mock<IFileMonitor>();
            FileMonitorRunner = new FileMonitorRunner(FileMonitor.Object);
        }

        [Test]
        public void TestStart()
        {
            const string dir = @"c:\something";
            const string pattern = "*.txt";
            const int interval = 5;
            var task = new Task(() =>
                {
                    FileMonitorRunner.Start(dir, pattern, interval);
                    FileMonitorRunner.Stop();
                    FileMonitor.Verify(m => m.Run(dir, pattern));
                });
        }
    }
}
