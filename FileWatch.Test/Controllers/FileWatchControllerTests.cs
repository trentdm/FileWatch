using System;
using System.Collections.Generic;
using System.IO;
using FileWatch.Controllers;
using FileWatch.Handlers;
using Moq;
using NUnit.Framework;

namespace FileWatch.Test.Controllers
{
    [TestFixture]
    public class FileWatchControllerTests
    {
        private IFileWatchController FileWatchController { get; set; }
        private Mock<IFileWatchHandler> FileWatchHandler { get; set; }
        private Mock<IFileMonitorRunner> FileMonitorRunner { get; set; }

        [SetUp]
        public void Setup()
        {
            FileWatchHandler = new Mock<IFileWatchHandler>();
            FileMonitorRunner = new Mock<IFileMonitorRunner>();
            FileWatchController = new FileWatchController(FileWatchHandler.Object, FileMonitorRunner.Object);
        }

        [Test]
        public void TestRun_ArgCount_ValidArgs()
        {
            var dirPath = Path.Combine(Path.GetTempPath(), "FileWatchTests", Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(dirPath);
            FileWatchController.Run(new []{dir.FullName, "*.txt"});
            dir.Delete(true);
        }

        [Test]
        public void TestRun_ArgCount_ValidRelativePath()
        {
            const string dirPath = @"thatpathwhichshallnotbenamed\relative\temp";
            var dir = Directory.CreateDirectory(dirPath);
            FileWatchController.Run(new[] { dirPath, "*.txt" });
            dir.Delete(true);
        }

        [Test]
        [Explicit("Requires accessible network with write permissions to UNC path")]
        public void TestRun_ArgCount_ValidUncPath()
        {
            const string dirPath = @"\\drpr-test\scratch\temp\thatpathwhichshallnotbenamed";
            var dir = Directory.CreateDirectory(dirPath);
            FileWatchController.Run(new[] { dirPath, "*.txt" });
            dir.Delete(true);
        }

        [Test]
        public void TestRun_ArgCount_ValidAbsolutePath()
        {
            var dirPath = Path.Combine(Path.GetTempPath(), "FileWatchTests", Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(dirPath);
            FileWatchController.Run(new[] { dir.FullName, "*.txt" });
            dir.Delete(true);
        }


        [Test]
        public void TestRun_ArgCount_WrongCount()
        {
            var args = new List<string>
                {
                    "*.txt"
                };
            Assert.Throws<ArgumentException>(() => FileWatchController.Run(args));
        }

        [Test]
        public void TestRun_ArgCount_InvalidDir()
        {
            const string dirPath = @"c:\temp\jammalamma";
            Assert.Throws<ArgumentException>(() => FileWatchController.Run(new[]{dirPath, "*.txt"}));
        }
    }
}
