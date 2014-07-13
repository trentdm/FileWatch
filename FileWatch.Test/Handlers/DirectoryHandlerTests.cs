using System.IO;
using System.Linq;
using FileWatch.Handlers;
using NUnit.Framework;

namespace FileWatch.Test.Handlers
{
    [TestFixture]
    public class DirectoryHandlerTests
    {
        private IDirectoryHandler DirectoryHandler { get; set; }

        [SetUp]
        public void Setup()
        {
            DirectoryHandler = new DirectoryHandler();
        }

        [Test]
        public void TestGetFiles()
        {
            var dirPath = Path.Combine(Path.GetTempPath(), "FileWatchTests", Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(dirPath);
            var file = new FileInfo(Path.Combine(dir.FullName, "foo.txt"));
            file.Create().Close();
            var result = DirectoryHandler.GetFiles(dirPath, "*");
            Assert.That(result.Single().FullName, Is.EqualTo(file.FullName));
            dir.Delete(true);
        }

        [Test]
        public void TestGetFiles_UsesPattern()
        {
            var dirPath = Path.Combine(Path.GetTempPath(), "FileWatchTests", Path.GetRandomFileName());
            var dir = Directory.CreateDirectory(dirPath);
            var file1 = new FileInfo(Path.Combine(dir.FullName, "foo.txt"));
            file1.Create().Close();
            var file2 = new FileInfo(Path.Combine(dir.FullName, "bar5.txt"));
            file2.Create().Close();
            var result = DirectoryHandler.GetFiles(dirPath, "bar*.txt");
            Assert.That(result.Single().FullName, Is.EqualTo(file2.FullName));
            dir.Delete(true);
        }
    }
}