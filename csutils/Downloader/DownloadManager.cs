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
        
        private List<IDownloader> downloads;

        /// <summary>
        /// All downloaders registered for this manager
        /// </summary>
        public IDownloader[] Downloads
        {
            get { return downloads.ToArray(); }
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

        public DownloadManager(IEnumerable<string> sources)
        {
             if (sources == null)
                throw new ArgumentException("sources can not be null");

            if (sources.Any(a => a == null))
                throw new ArgumentException("at least one of the sources is null");

            downloads = new List<IDownloader>();

            MaxParallelDownloads = 1;
            DownloaderFactory fac = new DownloaderFactory();
            foreach(string src in sources)
            {
                IDownloader d = fac.CreateDownloader(src);
                d.DownloadProgress += DownloadProgress;
                downloads.Add(d);
            }         
        }

        public DownloadManager(IEnumerable<IDownloader> downloaders)
        {
            if (downloaders == null)
                throw new ArgumentException("downloaders can not be null");

            if (downloaders.Any(a => a == null))
                throw new ArgumentException("at least one of the downloaders is null");

            downloads = new List<IDownloader>();

            MaxParallelDownloads = 1;
            downloads.AddRange(downloaders);

            foreach(var d in downloaders)
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
        public int MaxParallelDownloads
        {
            get { return parallel; }
            set 
            {
                if (value <= 0)
                    throw new ArgumentException("MaxParallelDownloads has to be at least 1");

                if(value >= parallel)
                    parallel = value; 
                else
                {
                    //suspend additional downloads if the new value is smaller than the old one
                    for (int i = 0, skipped=0; i < Downloads.Length; i++ )
                    {
                        if(Downloads[i].DownloaderState == DownloadState.Downloading)
                        {
                            if (skipped < value)
                                skipped++;
                            else
                                Downloads[i].Pause();
                        }
                    }
                    parallel = value;
                }
            }
        }



        public void StartAsync()
        {
            foreach(var dl in Pending.Take(MaxParallelDownloads-RunningDownloads.Count()))
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
