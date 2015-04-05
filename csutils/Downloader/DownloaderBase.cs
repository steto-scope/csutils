using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    /// <summary>
    /// Base class for all downloader
    /// </summary>
    internal abstract class DownloaderBase : Base//, IDownloader
    {

      

        public bool IsCompleted { get { return DownloaderState == DownloadState.Completed; } }

        public bool IsDownloading { get { return DownloaderState == DownloadState.Downloading; } }

       
        /// <summary>
        /// Progress in Percentage [0,100], Returns 0 when Filesize is unknown, StartAsync() only
        /// </summary>
        public double Percentage
        {
            get
            {
                if (TotalBytes == null)
                    return 0;

                return (double)DownloadedBytes / (double)TotalBytes.Value * 100.0;
            }
        }

        /// <summary>
        /// returns true, when the filesize is unknown. Call GetFileSize() or start download to make sure that the filesize is really unknown
        /// </summary>
        public bool HasUnknownFilesize
        {
            get
            {
                return TotalBytes == null;
            }
        }


        public event EventHandler<DownloadProgressEventArgs> DownloadProgress;


        protected void RaiseDownloadProgress(long bytes, DownloadState state)
        {
            if (DownloadProgress != null)
                DownloadProgress(this, new DownloadProgressEventArgs(bytes, state));
        }


        private long? totalbytes;
        public long? TotalBytes
        {
            get { return totalbytes; }
            protected set { totalbytes = value;
            OnPropertyChanged("TotalBytes");
            }
        }

        private DownloadState state;
        public DownloadState DownloaderState
        {
            get { return state; }
            protected set { state = value; OnPropertyChanged("DownloaderState"); }
        }

        public long DownloadedBytes
        {
            get;
            protected set;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }  
        
        public Exception Error
        {
            get;
            protected set;
        }

        public Stream Target
        {
            get;
            protected set;
        }

        public string SourceIdentifier
        {
            get;
            protected set;
        }


        public bool StreamStaysOpen
        {
            get;
            set;
        }
    }
}
