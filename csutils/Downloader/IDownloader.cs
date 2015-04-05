﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Downloader
{
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
        /// Current state of the downloader. 
        /// </summary>
        DownloadState DownloaderState
        {
            get;
        }

        long DownloadedBytes { get;  }

        /// <summary>
        /// Exception, if the Downloader failed downloading
        /// </summary>
        Exception Error { get; }
        /// <summary>
        /// Target where the received bytes are written. The stream gets never closed by the downloader. 
        /// Creation and closing has to be done by the caller.
        /// </summary>
        Stream Target { get;  }
        /// <summary>
        /// Source identifier. 
        /// Based on this property a suitable downloader is chosen by the factory and the origin of the file to download is specified
        /// </summary>
        string SourceIdentifier { get; }

        bool IsDownloading { get; }
        bool IsCompleted { get; }
        bool HasUnknownFilesize { get; }

        event EventHandler<DownloadProgressEventArgs> DownloadProgress;

    }
}
