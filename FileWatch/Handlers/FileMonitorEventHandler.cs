using System;
using FileWatch.Models;

namespace FileWatch.Handlers
{
    public delegate void FileMonitorEventHandler(object sender, FileMonitorEventArgs e);

    public class FileMonitorEventArgs : EventArgs
    {
        public FileMonitorEventStatus Status { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public FileMonitorEventArgs() { }

        public FileMonitorEventArgs(FileMonitorEventStatus status, string name, string fullName)
        {
            Status = status;
            Name = name;
            FullName = fullName;
        }
    }
}
