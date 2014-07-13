using System;
using NUnit.Framework;

namespace FileWatch.Test
{
    [TestFixture]
    public class FileWatchTests
    {
        private FileWatch FileWatch { get; set; }

        [SetUp]
        public void Setup()
        {
            FileWatch = new FileWatch();
        }

        [Test]
        public void TestRun_BootstrapsIntoController()
        {
            Assert.Throws<ArgumentException>(() => FileWatch.Run(new string[]{}));
        }
    }
}
