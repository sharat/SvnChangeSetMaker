// svn changeset helper
// Author: Sarath C 
// Email: csarath@gmail.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using SharpSvn;

namespace LibSvnChangeSet
{
    /// <summary>
    /// Background Worker information
    /// </summary>
    internal class WorkerInformation
    {
        public BackgroundWorker Worker { get; set; }
        public string LocalPath { get;set;}
        public EventHandler<ProgressEventArgs> ProgressHandler { get; set; }
        public EventHandler<CompletedEventArgs> CompletedHandler { get; set; }
    }

    /// <summary>
    /// Public class helps to retrieve the changeset information
    /// </summary>
    public class SvnHelper
    {
        private WorkerInformation bgWorkerInfo = null;

        /// <summary>
        /// create the final change-set with the given directory
        /// </summary>
        /// <param name="modifiedFileList">Modified file list</param>
        /// <param name="locaArchivePath">path to the local archive</param>
        /// <param name="targetChangeSetDir">directory path to write the file to</param>
        public bool createChangeList(List<string> modifiedFileList, string localArchivePath, string targetChangeSetDir)
        {
            try
            {
                if (modifiedFileList == null || modifiedFileList.Count == 0 ||
                    string.IsNullOrEmpty(targetChangeSetDir) || string.IsNullOrEmpty(localArchivePath))
                    return false;

                string newDirPath = targetChangeSetDir + @"\new";
                string oldDirPath = targetChangeSetDir + @"\old";
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

                                // Write new and old files to the respective directories
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
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
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
                bw.RunWorkerAsync();
                Console.ReadLine();

                return true;
            }
            return false;
        }

        #region Background Workers
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.bgWorkerInfo.CompletedHandler(sender, new CompletedEventArgs() { Message = "Completed" });
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.bgWorkerInfo.ProgressHandler(sender,
                new ProgressEventArgs() { FileName = (string)e.UserState, FileStatus = FileStatus.Updated });
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            string localPath = this.bgWorkerInfo.LocalPath;

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
        /// Get the revision of a specified file and write to the path specified
        /// </summary>
        /// <param name="fileWorkingCopyPath">local path for the file to get the specfied version</param>
        /// <param name="filePathToWrite">Path to write the new file</param>
        /// <param name="bWorkingCopy">Simply copy from source to destination. Or else get the base version from SVN</param>
        public void getFile(string fileWorkingCopyPath, string filePathToWrite, bool bWorkingCopy)
        {
            SvnChangeSetHelper.createDirForFile(filePathToWrite);
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
