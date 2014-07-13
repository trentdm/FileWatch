namespace FileWatch.Models
{
    public class WatchInfo
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public int LineCount { get; set; }

        public WatchInfo() { }

        public WatchInfo(string name, string fullPath, int lineCount)
        {
            Name = name;
            FullPath = fullPath;
            LineCount = lineCount;
        }
    }
}
