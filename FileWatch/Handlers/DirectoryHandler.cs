using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileWatch.Handlers
{
    public interface IDirectoryHandler
    {
        IList<FileInfo> GetFiles(string directory, string searchPattern);
    }

    public class DirectoryHandler : IDirectoryHandler
    {
        public IList<FileInfo> GetFiles(string directory, string searchPattern)
        {
            return Directory.GetFiles(directory, searchPattern).Select(f => new FileInfo(f)).ToList();
        }
    }
}
