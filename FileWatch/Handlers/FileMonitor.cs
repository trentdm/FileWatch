using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileWatch.Models;

namespace FileWatch.Handlers
{
    public interface IFileMonitor
    {
        void Run(string directory, string searchPattern);
        event FileMonitorEventHandler Created;
        event FileMonitorEventHandler Modified;
        event FileMonitorEventHandler Deleted;
    }

    public class FileMonitor : IFileMonitor
    {
        public event FileMonitorEventHandler Created;
        public event FileMonitorEventHandler Modified;
        public event FileMonitorEventHandler Deleted;

        private IDirectoryHandler DirectoryHandler { get; set; }
        private IList<FileInfo> FileList { get; set; }

        public FileMonitor(IDirectoryHandler directoryHandler)
        {
            DirectoryHandler = directoryHandler;
            FileList = new List<FileInfo>();
        }

        public void Run(string directory, string searchPattern)
        {
            var currentFiles = DirectoryHandler.GetFiles(directory, searchPattern);
            ProcessNewAndModifiedFiles(currentFiles);
            var deletedFiles = FileList.Except(currentFiles, new FileEqualityComparer());
            ProcessDeletedFiles(deletedFiles);
        }

        private void ProcessNewAndModifiedFiles(IEnumerable<FileInfo> currentFiles)
        {
            foreach (var currentFile in currentFiles)
            {
                var origFile = FileList.FirstOrDefault(f => new FileEqualityComparer().Equals(f, currentFile));

                if (origFile == null)
                {
                    FileList.Add(currentFile);
                    var handler = Created;
                    if(handler != null)
                        handler(this, new FileMonitorEventArgs(FileMonitorEventStatus.Created, currentFile.Name, currentFile.FullName));
                }
                else if (IsModified(origFile, currentFile))
                {
                    FileList.Remove(origFile);
                    FileList.Add(currentFile);
                    var handler = Modified;
                    if (handler != null)
                        handler(this, new FileMonitorEventArgs(FileMonitorEventStatus.Modified, currentFile.Name, currentFile.FullName));
                }
            }
        }

        private bool IsModified(FileInfo origFile, FileInfo currentFile)
        {
            return origFile.LastWriteTime != currentFile.LastWriteTime;
        }

        private void ProcessDeletedFiles(IEnumerable<FileInfo> deletedFiles)
        {
            var filesToRemove = new List<FileInfo>();

            foreach (var deletedFile in deletedFiles)
            {
                filesToRemove.Add(deletedFile);
                var handler = Deleted;
                if (handler != null)
                    handler(this, new FileMonitorEventArgs(FileMonitorEventStatus.Deleted, deletedFile.Name, deletedFile.FullName));
            }

            filesToRemove.ForEach(f => FileList.Remove(f));
        }

        private class FileEqualityComparer : IEqualityComparer<FileInfo>
        {
            public bool Equals(FileInfo x, FileInfo y)
            {
                return x.FullName.Equals(y.FullName, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(FileInfo obj)
            {
                return 0;
            }
        }
    }
}