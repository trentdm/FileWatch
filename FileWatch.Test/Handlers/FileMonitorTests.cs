using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FileWatch.Handlers;
using FileWatch.Models;
using Moq;
using NUnit.Framework;

namespace FileWatch.Test.Handlers
{
    [TestFixture]
    public class FileMonitorTests
    {
        private FileMonitor FileMonitor { get; set; }
        private Mock<IDirectoryHandler> DirectoryHandler { get; set; }
       
        [SetUp]
        public void Setup()
        {
            DirectoryHandler = new Mock<IDirectoryHandler>();
            FileMonitor = new FileMonitor(DirectoryHandler.Object);
        }

        [Test]
        public void TestStart_NewFile()
        {
            var files = new List<FileInfo> { new FileInfo("foo.txt") };
            const string dir = "fooDir";
            const string pattern = "*";
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(files);
            var eventArgs = new FileMonitorEventArgs();
            FileMonitor.Created +=  delegate(object sender, FileMonitorEventArgs e) { eventArgs = e; };
            FileMonitor.Run(dir, pattern);
            Assert.That(eventArgs.Status, Is.EqualTo(FileMonitorEventStatus.Created));
        }

        [Test]
        public void TestStart_ModifiedFile()
        {
            var file1 = new FileInfo("foo.txt");
            file1.Create().Close();
            var lastModified = file1.LastWriteTime;
            var files = new List<FileInfo> { file1 };
            const string dir = "fooDir";
            const string pattern = "*";
            var eventArgs = new FileMonitorEventArgs();
            FileMonitor.Modified += delegate(object sender, FileMonitorEventArgs e) { eventArgs = e; };
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(files);
            FileMonitor.Run(dir, pattern);
            var file2 = new FileInfo("foo.txt");
            file2.Create().Close();
            var files2 = new List<FileInfo> { file2 };
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(files2);
            FileMonitor.Run(dir, pattern);
            file1.Delete();
            file2.Delete();
            Assert.That(eventArgs.Status, Is.EqualTo(FileMonitorEventStatus.Modified));
        }

        [Test]
        public void TestStart_NoModifiedFile()
        {
            var files = new List<FileInfo> { new FileInfo("foo.txt") };
            const string dir = "fooDir";
            const string pattern = "*";
            var eventArgs = new FileMonitorEventArgs();
            FileMonitor.Modified += delegate(object sender, FileMonitorEventArgs e) { eventArgs = e; };
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(files);
            FileMonitor.Run(dir, pattern);
            FileMonitor.Run(dir, pattern);
            Assert.That(eventArgs.Status, Is.EqualTo(FileMonitorEventStatus.NoChange));
        }

        [Test]
        public void TestStart_DeleteFile()
        {
            var files = new List<FileInfo> { new FileInfo("foo.txt") };
            const string dir = "fooDir";
            const string pattern = "*";
            var eventArgs = new FileMonitorEventArgs();
            FileMonitor.Deleted += delegate(object sender, FileMonitorEventArgs e) { eventArgs = e; };
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(files);
            FileMonitor.Run(dir, pattern);
            DirectoryHandler.Setup(h => h.GetFiles(dir, pattern)).Returns(new List<FileInfo>());
            FileMonitor.Run(dir, pattern);
            Assert.That(eventArgs.Status, Is.EqualTo(FileMonitorEventStatus.Deleted));
        }
    }
}
