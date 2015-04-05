using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Downloader
{
    public enum DownloadState : int
    {
        Inactive = 0,
        Starting = 1,
        Started = 2,
        Downloading = 3,
        Paused = 4,
        Aborting = 5,
        Aborted = 6,
        Completed = 7,
        Error = 8
    }
}
