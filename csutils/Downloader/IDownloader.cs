using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Downloader
{
	/// <summary>
	/// Interface for all Downloader Implemetations
	/// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Starts the download asynchronously
        /// </summary>
        void StartAsync();
        /// <summary>
        /// Starts the download synchronously. Pause and Abort have no effect
        /// </summary>
        void Start();

        /// <summary>
        /// pauses the download, till Start() gets called again. 
        /// Pause() does not terminate the connection, it just stops receiving data.
        /// </summary>
        void Pause();

        /// <summary>
        /// Aborts the download. After cancellation the downloaded data still remains in the Target-Stream
        /// </summary>
        void Abort();

        /// <summary>
        /// Total size of the file (in bytes). When null, the size is unknown, StartAsync() only
        /// </summary>
        long? TotalBytes { get;  }


        /// <summary>
        /// Gets current state of the downloader. 
        /// </summary>
        DownloadState DownloaderState
        {
            get;
        }

		/// <summary>
		/// Amount of Bytes downloaded so far
		/// </summary>
        long DownloadedBytes { get;  }

        /// <summary>
        /// Exception, if the Downloader failed downloading
        /// </summary>
        Exception Error { get; }
        /// <summary>
        /// Target where the received bytes are written.
        /// </summary>
        Stream Target { get;  }
		/// <summary>
		/// Targetfile where the bytes are written. 
		/// </summary>
		string TargetFile { get; }
        /// <summary>
        /// Source identifier. 
        /// Based on this property a suitable downloader is chosen by the factory and the origin of the file to download is specified
        /// </summary>
        string SourceIdentifier { get; }
		/// <summary>
		/// True, if the Downloader is currently downloading
		/// </summary>
        bool IsDownloading { get; }
		/// <summary>
		/// True, if the download has been completed successfully
		/// </summary>
        bool IsCompleted { get; }
		/// <summary>
		/// True if the filesize is unknown. This can occour when the download is a stream or the size hasn´t been retrieved yet
		/// </summary>
        bool HasUnknownFilesize { get; }
		/// <summary>
		/// Gets or sets the bandwidth limitation for this downloader (in B/s)
		/// </summary>
		int BandwidthLimit { get; set; }
		/// <summary>
		/// Gets the percentage of the download progress. Range: [0, 100]
		/// </summary>
        double Percentage { get; }

		/// <summary>
		/// Event for notifying the caller about the progress. 
		/// </summary>
        event EventHandler<DownloadProgressEventArgs> DownloadProgress;

    }
}
