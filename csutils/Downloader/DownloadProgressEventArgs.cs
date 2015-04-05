using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    public class DownloadProgressEventArgs : EventArgs
    {
        public DownloadState State { get; private set; }
        public long Bytes { get; private set; }

        public DownloadProgressEventArgs(long bytes, DownloadState state)
        {
            Bytes = bytes;
            State = state;
        }
    }
}
