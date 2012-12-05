using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        Failed,
        Cancelled
    }

    /// <summary>
    /// File information
    /// </summary>
    public class SvnFileInfo
    {
        public string FileName { get; set; }
        public FileStatus Status { get; set; }
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
        public string ErrorMessage { get; set; }
    }
}
