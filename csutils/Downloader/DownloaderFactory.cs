using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace csutils.Downloader
{
    /// <summary>
    /// Factoryclass for downloader. Subclass this class to add your own downloader
    /// </summary>
    public class DownloaderFactory
    {
       
        /// <summary>
        /// Creates a Downloader based on the given Uri/Path. The factorymethod decides based on the path which downloader to be used
        /// </summary>
        /// <param name="path">A path or uri</param>
        /// <param name="target">Target for the data. If null, a new MemoryStream will be used and hold in Target-Property</param>
        /// <param name="credentials">Optional, Credentials for authentification</param>
        /// <returns></returns>
        public virtual IDownloader CreateDownloader(string path, Stream target = null, ICredentials credentials = null)
        {
            if(target == null)
                return new HTTPFileDownloader(path, new MemoryStream(), credentials);

            return new HTTPFileDownloader(path, target, credentials);
        }

        /// <summary>
        /// Creates a Downloader based on the given Uri/Path. The factorymethod decides based on the path which downloader to be used
        /// </summary>
        /// <param name="path">A path or uri</param>
        /// <param name="target">Target for the data (valid filepath). </param>
        /// <param name="credentials">Optional, Credentials for authentification</param>
        /// <returns></returns>
        public virtual IDownloader CreateDownloader(string path, string target, ICredentials credentials = null)
        {
            if (target == null)
                return new HTTPFileDownloader(path, new MemoryStream(), credentials);

            return new HTTPFileDownloader(path, target, credentials);
        }

        

    }
}
