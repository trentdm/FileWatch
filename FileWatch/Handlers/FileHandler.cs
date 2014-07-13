using System.Globalization;
using System.IO;

namespace FileWatch.Handlers
{
    public interface IFileHandler
    {
        int GetLineCount(string fullPath);
        string GetChange(int oldLineCount, int newLineCount);
    }

    public class FileHandler : IFileHandler
    {
        public int GetLineCount(string fullPath)
        {
            return SafeReadLineCount(fullPath);
        }

        public int SafeReadLineCount(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var count = 0;

                    while (!streamReader.EndOfStream)
                    {
                        streamReader.ReadLine();
                        count++;
                    }

                    return count;
                }
            }
        }

        public string GetChange(int oldLineCount, int newLineCount)
        {
            var difference = newLineCount - oldLineCount;

            if (difference >= 0)
                return "+" + difference;
            else
                return difference.ToString(CultureInfo.InvariantCulture);
        }
    }
}
