using System;
using System.IO;

namespace LibSvnChangeSet
{
    public class SvnChangeSetHelper
    {
        /// <summary>
        /// Do a simple zip of the given directory
        /// </summary>
        /// <param name="sourceDirPath"></param>
        /// <param name="targetZipPath"></param>
        /// <returns>True if succeeds</returns>
        public static bool zipChangeSetDir(string sourceDirPath, string targetZipPath)
        {
            try
            {
                if (string.IsNullOrEmpty(targetZipPath) || !Directory.Exists(sourceDirPath) || string.IsNullOrEmpty(sourceDirPath))
                    return false;

                createDirForFile(targetZipPath);

                using (Ionic.Zip.ZipFile zfile = new Ionic.Zip.ZipFile())
                {
                    zfile.AddDirectory(sourceDirPath);
                    
                    zfile.SaveProgress += new EventHandler<Ionic.Zip.SaveProgressEventArgs>(zfile_SaveProgress);
                    //string[] filenames = Directory.GetFiles(sourceDirPath, "*.*", SearchOption.AllDirectories);
                    //zfile.AddFiles(filenames, true, string.Empty);

                    string zipFilePath = targetZipPath;
                    using (FileStream file = new FileStream(zipFilePath, FileMode.Create))
                    {
                        zfile.Save(file);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Progress of the zip operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void zfile_SaveProgress(object sender, Ionic.Zip.SaveProgressEventArgs e)
        {
            
        }

        #region helper functions
        internal static void createDirForFile(string fullFilePath)
        {
            string dirPath = Path.GetDirectoryName(fullFilePath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        public static void deletePath(string fullFilePath)
        {
            string dirPath = Path.GetDirectoryName(fullFilePath);
            try
            {
                if(Directory.Exists(fullFilePath))
                    Directory.Delete( fullFilePath, true);
                else
                    File.Delete(fullFilePath);
            }
            catch
            {

            }
        }
        #endregion
    }
}
