using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using FileWatch.Models;
using FileWatch.Utils;

namespace FileWatch.Handlers
{
    public interface IFileWatchHandler
    {
        void Created(object sender, FileMonitorEventArgs e);
        void Modified(object sender, FileMonitorEventArgs e);
        void Deleted(object sender, FileMonitorEventArgs e);
    }

    public class FileWatchHandler : IFileWatchHandler
    {
        private IObjectFactory ObjectFactory { get; set; }
        private ConcurrentDictionary<string, WatchInfo> WatchInfos { get; set; }

        public FileWatchHandler(IObjectFactory objectFactory)
        {
            ObjectFactory = objectFactory;
            WatchInfos = new ConcurrentDictionary<string, WatchInfo>();
        }

        public void Created(object sender, FileMonitorEventArgs e)
        {
            Task.Factory.StartNew(() => HandleCreated(e.Name, e.FullName));
        }

        private void HandleCreated(string name, string fullPath)
        {
            var handler = ObjectFactory.GetInstance<IFileHandler>();
            var lineCount = handler.GetLineCount(fullPath);
            AddWatchInfos(name, new WatchInfo(name, fullPath, lineCount));
            this.LogInfo("{0} {1}", name, lineCount.ToString(CultureInfo.InvariantCulture));
        }

        private void AddWatchInfos(string name, WatchInfo watchInfo)
        {
            WatchInfos.TryAdd(name.ToUpper(), watchInfo);
        }

        public void Modified(object sender, FileMonitorEventArgs e)
        {
            Task.Factory.StartNew(() => HandleChanged(e.Name, e.FullName));
        }

        private void HandleChanged(string name, string fullPath)
        {
            var handler = ObjectFactory.GetInstance<IFileHandler>();
            var currentLineCount = handler.GetLineCount(fullPath);
            var watchInfo = GetWatchInfo(name);
            var change = handler.GetChange(watchInfo.LineCount, currentLineCount);
            UpdateLineCount(watchInfo, currentLineCount);
            this.LogInfo("{0} {1}", name, change);
        }

        private WatchInfo GetWatchInfo(string name)
        {
            WatchInfo watchInfo;
            WatchInfos.TryGetValue(name.ToUpper(), out watchInfo);
            return watchInfo;
        }

        private void UpdateLineCount(WatchInfo watchInfo, int lineCount)
        {
            watchInfo.LineCount = lineCount;
        }

        public void Deleted(object sender, FileMonitorEventArgs e)
        {
            Task.Factory.StartNew(() => HandleDeleted(e.Name));
        }

        private void HandleDeleted(string name)
        {
            RemoveWatchInfos(name);
            this.LogInfo("{0}", name);
        }

        private void RemoveWatchInfos(string name)
        {
            WatchInfo watchInfo;
            WatchInfos.TryRemove(name.ToUpper(), out watchInfo);
        }
    }
}