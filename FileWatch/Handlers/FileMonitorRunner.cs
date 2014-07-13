using System.Threading;

namespace FileWatch.Handlers
{
    public interface IFileMonitorRunner
    {
        event FileMonitorEventHandler Created;
        event FileMonitorEventHandler Modified;
        event FileMonitorEventHandler Deleted;
        void Start(string directory, string searchPattern, int intervalMilliseconds);
        void Stop();
    }

    public class FileMonitorRunner : IFileMonitorRunner
    {
        public event FileMonitorEventHandler Created
        {
            add { FileMonitor.Created += value; }
            remove { FileMonitor.Created -= value; }
        }
        public event FileMonitorEventHandler Modified
        {
            add { FileMonitor.Modified += value; }
            remove { FileMonitor.Modified -= value; }
        }
        public event FileMonitorEventHandler Deleted
        {
            add { FileMonitor.Deleted += value; }
            remove { FileMonitor.Deleted -= value; }
        }

        private IFileMonitor FileMonitor { get; set; }
        private string Directory { get; set; }
        private string SearchPattern { get; set; }
        private Timer Timer { get; set; }

        public FileMonitorRunner(IFileMonitor fileMonitor)
        {
            FileMonitor = fileMonitor;
        }

        public void Start(string directory, string searchPattern, int intervalMilliseconds)
        {
            Directory = directory;
            SearchPattern = searchPattern;
            Timer = new Timer(Run, new AutoResetEvent(false), 0, intervalMilliseconds);
        }

        private void Run(object stateInfo)
        {
            FileMonitor.Run(Directory, SearchPattern);
            ((AutoResetEvent) stateInfo).Set();
        }

        public void Stop()
        {
            Timer.Dispose();
        }
    }
}
