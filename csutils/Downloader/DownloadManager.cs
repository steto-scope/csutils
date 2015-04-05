using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    /// <summary>
    /// Manager for multiple downloads, based on the workpile pattern
    /// </summary>
    public class DownloadManager 
    {
        
        private IDownloader[] donwloads;

        /// <summary>
        /// All downloaders registered for this manager
        /// </summary>
        public IDownloader[] Downloads
        {
            get { return donwloads; }
            protected set { donwloads = value; }
        }

        /// <summary>
        /// all currently running downloads
        /// </summary>
        public IEnumerable<IDownloader> RunningDownloads
        {
            get
            {
                return Downloads.Where(w => w.IsDownloading);
            }
        }

        public event EventHandler Completed;

        /// <summary>
        /// all pending downloads
        /// </summary>
        public IEnumerable<IDownloader> Pending
        {
            get
            {
                return Downloads.Where(w => w.DownloaderState == DownloadState.Inactive || w.DownloaderState == DownloadState.Paused);
            }
        }

        public DownloadManager(IDownloader[] downloader)
        {
            if (downloader == null)
                throw new ArgumentException("array can not be null");

            if (downloader.Any(a => a == null))
                throw new ArgumentException("at least one of the downloaders is null");

            Parallel = 1;
            Downloads = downloader;

            foreach(var d in downloader)
                d.DownloadProgress += DownloadProgress;
        }

        void DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if(e.State == DownloadState.Completed)
            {
                if (Pending.Count() > 0)
                    StartAsync();

                if (Downloads.All(a => a.IsCompleted))
                    if (Completed != null)
                        Completed(this, EventArgs.Empty);
            }
        }

        private int parallel;
        /// <summary>
        /// Amount of parallel downloads
        /// </summary>
        public int Parallel
        {
            get { return parallel; }
            set { parallel = value; }
        }



        public void StartAsync()
        {
            foreach(var dl in Pending.Take(Parallel-RunningDownloads.Count()))
            {
                dl.StartAsync();
            }
        }

        public void Pause()
        {
            foreach (var dl in RunningDownloads)
                dl.Pause();
        }

        public void Abort()
        {
            foreach (var dl in RunningDownloads)
                dl.Abort();
        }


        public long DownloadedBytes
        {
            get
            {
                return Downloads.Sum(d => d.DownloadedBytes);
            }
        }

        public long? TotalBytes
        {
            get
            {
                if (Downloads.Any(a => a.HasUnknownFilesize))
                    return null;

                return Downloads.Sum(d => d.TotalBytes);
            }
        }

    }
}
