using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extensions for integers
    /// </summary>
    public static class csutilsIntExtension
    {
        /// <summary>
        /// returns 1 when value is positive, -1 if negative and 0 if value 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Saturate(this int value)
        {
            return value < 0 ? -1 : (value > 0 ? 1 : 0);
        }


    }
}
