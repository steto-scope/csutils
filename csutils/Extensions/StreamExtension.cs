using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension class for streams
    /// </summary>
    public static class csutilsStreamExtension
    {
        /// <summary>
        /// the buffer size used for operations
        /// </summary>
        public static int buffersize = 8 * 1024;

        /// <summary>
        /// reads the content of a stream into a string without closing it
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToString(this Stream stream, Encoding encoding)
        {
            if (!stream.CanSeek)
                return null;

            long pos = stream.Position;
            stream.Position = 0;
            int read=1;
            byte[]  buffer = new byte[buffersize];

            StringBuilder sb = new StringBuilder();
            while(read>0)
            {
                read = stream.Read(buffer, 0, buffer.Length);
                sb.Append(encoding.GetString(buffer,0,read));
            }
            
            stream.Position = pos;

            return sb.ToString();
        }
    }
}
