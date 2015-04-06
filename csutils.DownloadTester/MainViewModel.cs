using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.DownloadTester
{
    public class MainViewModel : Base
    {
        string[] urls = new string[] { "http://192.168.178.39/test.msi", "http://192.168.178.39/test.rar", "http://192.168.178.39/test.exe" };
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        private Downloader.DownloadManager dl;

        public Downloader.DownloadManager DownloadManager
        {
            get { return dl; }
            set { dl = value; OnPropertyChanged("DownloadManager"); }
        }

        public MainViewModel()
        {
            Downloader.DownloadManager m = new Downloader.DownloadManager(urls);           
            DownloadManager = m;
            DownloadManager.MaxParallelDownloads = 2;
			DownloadManager.BandwidthLimit = 1024*128;
        }
    }
}
