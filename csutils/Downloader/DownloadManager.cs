using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    /// <summary>
    /// Manager for multiple downloads, based on the workpile pattern
    /// </summary>
    public class DownloadManager : Base
    {
        
        private IDownloader[] downloads;

        /// <summary>
        /// All downloaders registered for this manager
        /// </summary>
        public IDownloader[] Downloads
        {
            get { return downloads; }
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

		/// <summary>
		/// Event that is fired if the DownloadManager finished it´s work
		/// </summary>
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

		/// <summary>
		/// Creates a new DownloadManager based on sources-strings. These can be URLs.
		/// The Default-Implementation of the DownloaderFactory get used to create the downloaders
		/// </summary>
		/// <param name="sources"></param>
        public DownloadManager(IEnumerable<string> sources)
        {
             if (sources == null)
                throw new ArgumentException("sources can not be null");

            if (sources.Any(a => a == null))
                throw new ArgumentException("at least one of the sources is null");

			downloads = new IDownloader[sources.Count()];

            MaxParallelDownloads = 1;
            DownloaderFactory fac = new DownloaderFactory();
			int i = 0;
            foreach(string src in sources)
            {
                IDownloader d = fac.CreateDownloader(src);
                d.DownloadProgress += DownloadProgress;
                downloads[i] = d;
				i++;
            }         
        }

		/// <summary>
		/// Creates the DownloadManager with the given Downloaders
		/// </summary>
		/// <param name="downloaders"></param>
        public DownloadManager(IEnumerable<IDownloader> downloaders)
        {
            if (downloaders == null)
                throw new ArgumentException("downloaders can not be null");

            if (downloaders.Any(a => a == null))
                throw new ArgumentException("at least one of the downloaders is null");
			
            MaxParallelDownloads = 1;
			downloads = downloaders.ToArray();

            foreach(var d in downloaders)
                d.DownloadProgress += DownloadProgress;
        }

        void DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if(e.State == DownloadState.Completed)
            {
                if (Pending.Count() > 0)
                    Start();

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

                OnPropertyChanged("MaxParallelDownloads");
            }
        }

		private int bandwidthlimit = int.MaxValue;
		/// <summary>
		/// Bandwidth-Limit for all Downloaders combined (in B/s). The Limit will be distributed evenly to all active Downloaders
		/// </summary>
		public int BandwidthLimit
		{
			get { return bandwidthlimit; }
			set 
			{
				bandwidthlimit = value;
				ApplyDownloadLimits();
			}
		}

		private void ApplyDownloadLimits()
		{
			if(RunningDownloads.Count()<1)
				return;

			int limit = BandwidthLimit/RunningDownloads.Count();
			foreach (IDownloader dl in RunningDownloads)
				dl.BandwidthLimit = limit;
		}

		/// <summary>
		/// Starts/Resumes the downloads of this manager
		/// </summary>
        public void Start()
        {
            foreach(var dl in Pending.Take(MaxParallelDownloads-RunningDownloads.Count()))
            {
                dl.StartAsync();
            }
			ApplyDownloadLimits();
        }

		/// <summary>
		/// Pauses all currently running downloads 
		/// </summary>
        public void Pause()
        {
            Console.WriteLine(RunningDownloads.Count());
            foreach (var dl in RunningDownloads)
                dl.Pause();
        }

		/// <summary>
		/// Aborts currently running downloads
		/// </summary>
        public void Abort()
        {
            foreach (var dl in RunningDownloads)
                dl.Abort();
        }

		/// <summary>
		/// The total amount of downloaded bytes
		/// </summary>
        public long DownloadedBytes
        {
            get
            {
                return Downloads.Sum(d => d.DownloadedBytes);
            }
        }

		/// <summary>
		/// The total amount of bytes for all downloads. Returns null if there is any download where the size can not be determined
		/// </summary>
        public long? TotalBytes
        {
            get
            {
                if (Downloads.Any(a => a.HasUnknownFilesize))
                    return null;

                return Downloads.Sum(d => d.TotalBytes);
            }
        }

		/// <summary>
		/// Clones the DownloadManager. Not Implemented
		/// </summary>
		/// <returns></returns>
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
