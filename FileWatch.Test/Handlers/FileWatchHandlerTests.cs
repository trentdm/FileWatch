using System;
using System.Threading;
using FileWatch.Handlers;
using FileWatch.Models;
using FileWatch.Utils;
using Moq;
using NUnit.Framework;

namespace FileWatch.Test.Handlers
{
    [TestFixture]
    public class FileWatchHandlerTests
    {
        private IFileWatchHandler FileWatchHandler { get; set; }
        private Mock<IFileHandler> FileHandler { get; set; }
        private Mock<IObjectFactory> ObjectFactory { get; set; }

        [SetUp]
        public void Setup()
        {
            FileHandler = new Mock<IFileHandler>();
            ObjectFactory = new Mock<IObjectFactory>();
            ObjectFactory.Setup(f => f.GetInstance<IFileHandler>()).Returns(FileHandler.Object);
            FileWatchHandler = new FileWatchHandler(ObjectFactory.Object);
        }

        [Test]
        public void TestCreated_GetLineCount()
        {
            FileWatchHandler.Created(this, new FileMonitorEventArgs(FileMonitorEventStatus.Created, "foo", "bar"));
            FileHandler.Verify(h => h.GetLineCount(It.IsAny<string>()));
        }

        [Test]
        public void TestCreated_Created()
        {
            FileWatchHandler.Created(this, new FileMonitorEventArgs(FileMonitorEventStatus.Created, "foo", "bar"));
        }

        [Test]
        public void TestOnNext_Modified()
        {
            FileWatchHandler.Created(this, new FileMonitorEventArgs(FileMonitorEventStatus.Created, "foo", "bar"));
            Thread.Sleep(500);
            FileWatchHandler.Modified(this, new FileMonitorEventArgs(FileMonitorEventStatus.Modified, "foo", "bar"));
            FileHandler.Verify(h => h.GetChange(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void TestOnNext_Deleted()
        {
            FileWatchHandler.Created(this, new FileMonitorEventArgs(FileMonitorEventStatus.Created, "foo", "bar"));
            Thread.Sleep(500);
            FileWatchHandler.Deleted(this, new FileMonitorEventArgs(FileMonitorEventStatus.Deleted, "foo", "bar"));
        }
    }
}
