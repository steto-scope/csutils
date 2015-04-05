using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace csutils.Downloader
{
    internal class HTTPFileDownloader : DownloaderBase, IDownloader
    {
        protected static int buffersize = 1024 * 64;
        /// <summary>
        /// Size of the receiving buffer. Any change needs a cancellation of a running download
        /// </summary>
        public static int BufferSize
        {
            get { return buffersize; }
            set
            {
                buffersize = value;
            }
        }

        private ICredentials credentials;
        /// <summary>
        /// Credentials used for authentification. If null, default credentials will be used
        /// </summary>
        public ICredentials Credentials
        {
            get { return credentials; }
            protected set { credentials = value; }
        }

        protected static int notifyevery = 1;
        /// <summary>
        /// Notifies every BufferSize*NotifyEvery bytes the caller about changes
        /// </summary>
        public static int NotifyInterval
        {
            get { return notifyevery; }
            set { notifyevery = value; }
        }

        private string targetPath;

        protected string TargetPath
        {
            get { return targetPath; }
            set { targetPath = value; }
        }


        /// <summary>
        /// Constructor is internal, because DownloaderFactory should be used to create a downloader
        /// </summary>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="credentials"></param>
        internal HTTPFileDownloader(string url, Stream target, ICredentials credentials)
        {
            Target = target;
            SourceIdentifier = url;
            Credentials = credentials;
        }

        /// <summary>
        /// Constructor is internal, because DownloaderFactory should be used to create a downloader
        /// </summary>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="credentials"></param>
        internal HTTPFileDownloader(string url, string target, ICredentials credentials)
        {
            if (target == null)
                throw new Exception("target have to be a valid filepath");

            TargetPath = target;
            SourceIdentifier = url;
            Credentials = credentials;
        }

        protected WebClient wc;
        protected Stream src;
        protected BackgroundWorker bw;

        public void StartAsync()
        {
            if (DownloaderState == DownloadState.Inactive || DownloaderState == DownloadState.Completed || DownloaderState == DownloadState.Error)
            {
                DownloaderState = DownloadState.Starting;
                RaiseDownloadProgress(0, DownloadState.Starting);

                Reset();

                bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.ProgressChanged += bw_ProgressChanged;

               
                RaiseDownloadProgress(0, DownloadState.Started);
                DownloaderState = DownloadState.Downloading;
                bw.RunWorkerAsync();
            }
            else if (DownloaderState == DownloadState.Paused)
            {
                DownloaderState = DownloadState.Downloading;
                RaiseDownloadProgress(0, DownloadState.Downloading);
            }
        }

        public void Start()
        {
            if (DownloaderState == DownloadState.Inactive || DownloaderState == DownloadState.Completed || DownloaderState == DownloadState.Error)
            {
                DoWorkEventArgs args = null;
                try
                {
                    DownloaderState = DownloadState.Starting;
                    RaiseDownloadProgress(0, DownloadState.Starting);

                    Reset();
                                       
                    RaiseDownloadProgress(0, DownloadState.Started);
                    DownloaderState = DownloadState.Downloading;

                    args = new DoWorkEventArgs(null);
                    bw_DoWork(this, args);
                }
                catch(Exception ex)
                {
                    Error = ex;
                }

                if (Error != null)
                {
                    DownloaderState = DownloadState.Error;
                    RaiseDownloadProgress(DownloadedBytes, DownloadState.Error);
                    return;
                }

                if (args!= null && args.Cancel==true)
                {
                    RaiseDownloadProgress(DownloadedBytes, DownloadState.Aborted);
                    Reset();
                    return;
                }

                DownloaderState = DownloadState.Completed;
                RaiseDownloadProgress(DownloadedBytes, DownloadState.Completed);
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DownloadedBytes += e.ProgressPercentage;
            OnPropertyChanged("DownloadedBytes");
            OnPropertyChanged("Percentage");
            RaiseDownloadProgress(DownloadedBytes, DownloadState.Downloading);
        }

        public void Abort()
        {
            if (DownloaderState == DownloadState.Started || DownloaderState == DownloadState.Paused)
            {
                DownloaderState = DownloadState.Aborting;
                RaiseDownloadProgress(DownloadedBytes, DownloadState.Aborting);
            }
        }

        protected void Reset()
        {
            if (bw != null && !bw.IsBusy)
            {
                bw.Dispose();
                bw = null;
                src = null;
                Target.Seek(0, SeekOrigin.Begin);
                DownloadedBytes = 0;
                DownloaderState = DownloadState.Inactive;
                if (TargetPath != null && Target == null)
                    Target = File.OpenWrite(TargetPath);
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Error = e.Error;
                DownloaderState = DownloadState.Error;
                RaiseDownloadProgress(DownloadedBytes, DownloadState.Error);
                return;
            }

            if (e.Cancelled)
            {
                RaiseDownloadProgress(DownloadedBytes, DownloadState.Aborted);
                DownloaderState = DownloadState.Aborted;
                Reset();               
                
                
                return;
            }       

            DownloaderState = DownloadState.Completed;
            RaiseDownloadProgress(DownloadedBytes, DownloadState.Completed);

            //always close FileStreams after completion
            if(Target is FileStream)
            {
                string name = ((FileStream)Target).Name;
                Target.Close();
                Target.Dispose();
                if(DownloaderState == DownloadState.Aborted || DownloaderState == DownloadState.Error)
                {
                    try
                    {
                        File.Delete(name);
                    }
                    catch { }
                }
            }
            else
            {
                if (DownloaderState == DownloadState.Aborted || DownloaderState == DownloadState.Error)
                {
                    Target.Close();
                    Target.Dispose();
                }
            }

        }

        /// <summary>
        /// Gets the size of the file. When null, the size is unknown (happens when the file is created on demand).
        /// The method downloads the file header to retrieve the size. Thus, it should be called in a background-thread.
        /// The method is meant to retrieve the size before downloading has started. When starting the download the size gets updated anyway.
        /// </summary>
        /// <returns></returns>
        public long? GetFileSize()
        {
            long? size = null;

            WebClient fswc = new WebClient();

            if (Credentials != null)
                fswc.Credentials = Credentials;

            using (Stream sr = wc.OpenRead(SourceIdentifier))
            {
                if (!string.IsNullOrEmpty(wc.ResponseHeaders["Content-Length"]))
                    size = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
                sr.Close();
            }
            
            return size;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            wc = new WebClient();

            if (Credentials != null)
                wc.Credentials = Credentials;

            src = wc.OpenRead(SourceIdentifier);
            if(!string.IsNullOrEmpty(wc.ResponseHeaders["Content-Length"]))
                TotalBytes = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
            byte[] buffer = new byte[BufferSize];

            int i = 0;
            int read = 1;
            while (DownloaderState != DownloadState.Aborting && read > 0)
            {
                if (DownloaderState == DownloadState.Paused)
                    Thread.Sleep(100);
                else
                {
                    read = src.Read(buffer, 0, buffer.Length);
                    if (read > 0)
                        Target.Write(buffer, 0, read);
                    i++;

                    if (i % NotifyInterval == 0 && bw!=null && bw.IsBusy)
                        bw.ReportProgress(read);
                }
            }
            Target.Flush();
            Target.Seek(0, SeekOrigin.Begin);

            if (DownloaderState == DownloadState.Aborting)
                e.Cancel = true;
        }





        public void Pause()
        {
            if (DownloaderState == DownloadState.Downloading)
            {
                DownloaderState = DownloadState.Paused;
                RaiseDownloadProgress(DownloadedBytes, DownloadState.Paused);
            }
        }       
    }
}
