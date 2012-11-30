// svn changeset helper
// Author: Sarath C 
// Email: csarath@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSvn;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LibSvnChangeSet
{
    /// <summary>
    /// Status of the file
    /// </summary>
    public enum FileStatus
    {
        Added,
        Deleted,
        Updated
    }

    /// <summary>
    /// Operation Status. Mostly used for getting the modification list.
    /// </summary>
    public enum OperationStatus
    {
        Success,
        InvalidArchive,
        Error,
        Cancelled
    }

    /// <summary>
    /// Progress callback parameters
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public FileStatus FileStatus { get; set; }
    }

    /// <summary>
    /// Completed event args
    /// </summary>
    public class CompletedEventArgs : EventArgs
    {
        public OperationStatus Status { get; set; }
        public string Message { get; set; }
    }
    
    /// <summary>
    /// Background Worker information
    /// </summary>
    public class WorkerInformation
    {
        public BackgroundWorker Worker { get; set; }
        public string LocalPath { get;set;}
        public EventHandler<ProgressEventArgs> ProgressHandler { get; set; }
        public EventHandler<CompletedEventArgs> CompletedHandler { get; set; }
    }

    public class SvnHelper
    {
        private WorkerInformation bgWorkerInfo = null;

        /// <summary>
        /// create the final change-set with the given directory
        /// </summary>
        /// <param name="modifiedFileList">Modified file list</param>
        /// <param name="dirPath">directory path to write the file to</param>
        /// <param name="locaArchivePath">path to the local archive</param>
        public bool createChangeList(List<string> modifiedFileList, string dirPath, string localArchivePath)
        {
            try
            {
                if (modifiedFileList == null || modifiedFileList.Count == 0 ||
                    string.IsNullOrEmpty(dirPath) || string.IsNullOrEmpty(localArchivePath))
                    return false;

                string newDirPath = dirPath + @"\new";
                string oldDirPath = dirPath + @"\old";
                Directory.CreateDirectory(newDirPath);
                Directory.CreateDirectory(oldDirPath);

                if (Directory.Exists(newDirPath) && Directory.Exists(oldDirPath))
                {
                    foreach (string file in modifiedFileList)
                    {
                        if (file.StartsWith(localArchivePath))
                        {
                            string subPath = file.Substring(localArchivePath.Length);
                            if (!string.IsNullOrEmpty(subPath))
                            {
                                string oldFilePath = oldDirPath + @"\" + subPath;
                                string newFilePath = newDirPath + @"\" + subPath;
                                Directory.CreateDirectory(Path.GetDirectoryName(oldFilePath));
                                Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                                getFile(file, newFilePath, true);
                                getFile(file, oldFilePath, false);
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                
            }
            return false;
        }

        /// <summary>
        /// Asynchronous way to get modified file list
        /// </summary>
        /// <param name="localPath">local archive path</param>
        /// <param name="progressCallback">Progress callback</param>
        /// <param name="completedCallback">Completed callback</param>
        /// <returns>true if successfully submits</returns>
        public bool getModifiedListAsync(string localPath, EventHandler<ProgressEventArgs> progressCallback, EventHandler<CompletedEventArgs> completedCallback)
        {
            if (string.IsNullOrEmpty(localPath) || null == progressCallback || null == completedCallback)
                return false;

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_GetModifiedFileList);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bgWorkerInfo = new WorkerInformation()
            {
                LocalPath = localPath,
                Worker = bw,
                CompletedHandler = completedCallback,
                ProgressHandler = progressCallback 
            };

            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync(new WorkerInformation() { LocalPath = localPath, Worker = bw });
                return true;
            }
            return false;
        }
        #region Background Workers
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bgWorkerInfo.ProgressHandler(sender,
                new ProgressEventArgs() { FileName = (string)e.UserState, FileStatus = FileStatus.Updated });
        }

        private void bw_GetModifiedFileList(object sender, DoWorkEventArgs e)
        {
            WorkerInformation info = sender as WorkerInformation;
            BackgroundWorker bw = info.Worker;
            string localPath = info.LocalPath;

            List<string> fileList = new List<string>();
            using (SvnClient client = new SvnClient())
            {
                client.Status(localPath,
                 delegate(object svnsender, SvnStatusEventArgs svargs)
                 {
                     if (svargs.LocalContentStatus == SvnStatus.Modified ||
                         svargs.LocalContentStatus == SvnStatus.Added ||
                         svargs.LocalContentStatus == SvnStatus.Deleted ||
                         svargs.LocalContentStatus == SvnStatus.Merged)
                         bw.ReportProgress(0, svargs.FullPath);
                         fileList.Add(svargs.FullPath);
                     if (bw.CancellationPending)
                     {
                         e.Cancel = true;
                         return; // if user cancelled in btween!
                     }
                 });
            }
        }
        #endregion


        /// <summary>
        /// Get the list files changed in the archive
        /// </summary>
        /// <param name="localPath">Archive path</param>
        /// <returns></returns>
        public List<string> getModifiedFilePaths(string localPath)
        {
            List<string> fileList = new List<string>();
            using (SvnClient client = new SvnClient())
            {
                client.Status(localPath,
                 delegate(object sender, SvnStatusEventArgs e)
                 {
                     if (e.LocalContentStatus == SvnStatus.Modified ||
                         e.LocalContentStatus == SvnStatus.Added ||
                         e.LocalContentStatus == SvnStatus.Deleted || 
                         e.LocalContentStatus == SvnStatus.Merged)
                         fileList.Add(e.FullPath);
                 });
            }
            return fileList;
        }

        /// <summary>
        /// The working copy's revision
        /// </summary>
        /// <param name="localPath">local archive path</param>
        /// <param name="pathToWrite"></param>
        /// <returns></returns>
        public long getArchiveRevision(string localPath)
        {
            using (SvnWorkingCopyClient workingCopyClient = new SvnWorkingCopyClient())
            {
                SvnWorkingCopyVersion version;
                workingCopyClient.GetVersion(localPath, out version);
                return version.End;
            }
        }

        /// <summary>
        /// Get the revision of a specified file and write to the path specified
        /// </summary>
        /// <param name="fileWorkingCopyPath">local path for the file to get the specfied version</param>
        /// <param name="filePathToWrite">Path to write the new file</param>
        /// <param name="bWorkingCopy">Simply copy from source to destination. Or else get the base version from SVN</param>
        public void getFile(string fileWorkingCopyPath, string filePathToWrite, bool bWorkingCopy)
        {
            if (bWorkingCopy) // Copy the file the the destination. This is used for old files typically.
                File.Copy(fileWorkingCopyPath, filePathToWrite);
            else
            {
                using (SvnClient c = new SvnClient())
                {
                    using (Stream to = File.Create(filePathToWrite))
                    {
                        c.Write(new SvnPathTarget(fileWorkingCopyPath, SvnRevision.Base), to);
                    }
                }
            }
        }
    }
}
