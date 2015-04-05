using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    /// <summary>
    /// Represents the State of a Download / the Downloader
    /// </summary>
    public enum DownloadState : int
    {
        /// <summary>
        /// The Download hasn´t started so far
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// Start() has been called and the Downloader prepares in order to begin the download. This state normally lasts very short
        /// </summary>
        Starting = 1,
        /// <summary>
        /// The download started. Will only sent by the Progress-Event. The state in the IDownloader.DownloaderState immediately jumps to Downloading instead
        /// </summary>
        Started = 2,
        /// <summary>
        /// The download is in progress
        /// </summary>
        Downloading = 3,
        /// <summary>
        /// The download is suspended
        /// </summary>
        Paused = 4,
        /// <summary>
        /// The User invoked the Abort()-Method to stop the download manually
        /// </summary>
        Aborting = 5,
        /// <summary>
        /// The Download has been aborted. This state stays until a reset.
        /// </summary>
        Aborted = 6,
        /// <summary>
        /// The Download has been finished successfully
        /// </summary>
        Completed = 7,
        /// <summary>
        /// The Download aborted with an error (i.e. Network Problems, no access or something else)
        /// </summary>
        Error = 8
    }
}
