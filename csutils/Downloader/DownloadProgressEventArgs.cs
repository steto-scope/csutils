using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
	/// <summary>
	/// EventArgs for reporting Progress Updates
	/// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
		/// <summary>
		/// State of the Downloader
		/// </summary>
        public DownloadState State { get; private set; }
		/// <summary>
		/// Bytes written
		/// </summary>
        public long Bytes { get; private set; }
		/// <summary>
		/// Creates new Object
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="state"></param>
        public DownloadProgressEventArgs(long bytes, DownloadState state)
        {
            Bytes = bytes;
            State = state;
        }
    }
}
