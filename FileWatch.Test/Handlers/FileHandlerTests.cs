using System;
using System.IO;
using FileWatch.Handlers;
using NUnit.Framework;

namespace FileWatch.Test.Handlers
{
    [TestFixture]
    public class FileHandlerTests
    {
        private IFileHandler FileHandler { get; set; }

        [SetUp]
        public void Setup()
        {
            FileHandler = new FileHandler();
        }

        [Test]
        public void TestGetLineCount()
        {
            var dirPath = Path.Combine(Path.GetTempPath(), "FileWatchTests", Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(dirPath);
            var file = new FileInfo(Path.Combine(dir.FullName, "foo.txt"));
            using (var fileStream = file.Create())
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine("Holy moly a line!");
                    streamWriter.WriteLine("Ohhh yeah, and another!");
                }
            }
            var result = FileHandler.GetLineCount(file.FullName);
            Assert.That(result, Is.EqualTo(2));
            dir.Delete(true);
        }

        [Test]
        public void TestGetChange_Same()
        {
            var result = FileHandler.GetChange(5, 5);
             Assert.That(result, Is.EqualTo("+0"));
        }

        [Test]
        public void TestGetChange_More()
        {
            var result = FileHandler.GetChange(2, 5);
            Assert.That(result, Is.EqualTo("+3"));
        }

        [Test]
        public void TestGetChange_Less()
        {
            var result = FileHandler.GetChange(10, 2);
            Assert.That(result, Is.EqualTo("-8"));
        }
    }
}
