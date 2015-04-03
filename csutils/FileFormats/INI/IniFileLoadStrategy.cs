using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.FileFormats.INI
{
    /// <summary>
    /// Strategy that describes how 2 ini files should be merged
    /// </summary>
    public enum IniFileLoadStrategy
    {
        /// <summary>
        /// discard the old file and replace it with the new data
        /// </summary>
        Replace,
        /// <summary>
        /// add sections and keys/values which are non-existent in the old file. Does not update values of existing keys
        /// </summary>
        Add,
        /// <summary>
        /// update values only. no new sections/keys are added
        /// </summary>
        Update,
        /// <summary>
        /// adds and updates sections and keys/values (complete merge) 
        /// </summary>
        Merge
    }
}
