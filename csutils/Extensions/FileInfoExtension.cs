using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System
{
	/// <summary>
	/// FileInfoExtensions
	/// </summary>
	public static class csutilsFileInfoExtension
	{
#if CLR
		/// <summary>
		/// Checks if Write-Permissions for this Location are granted
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool CanWrite(this FileInfo info)
		{
			try
			{
				return info.Directory.CanWrite();
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Calculates the MD5 hash of the file
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static string ToMD5(this FileInfo info)
		{
			return File.ReadAllBytes(info.FullName).ToMD5();
		}

		/// <summary>
		/// Calculates the SHA1 hash of the file
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static string ToSHA1(this FileInfo info)
		{
			return File.ReadAllBytes(info.FullName).ToSHA1();
		}

        /// <summary>
        /// Checks if the file is a descendant of a directory
        /// </summary>
        /// <param name="fsi"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsDescendantOf(this FileInfo fsi, DirectoryInfo dir)
        {
            if (dir == null)
                throw new ArgumentNullException("dir");

            string target = dir.FullName.EndsWith("\\") ? dir.FullName.Substring(0, dir.FullName.Length - 1) : dir.FullName;

            if (fsi.Directory.FullName == dir.FullName)
                return true;

            DirectoryInfo temp = new DirectoryInfo(fsi.Directory.FullName);

            while (temp.Parent != null)
            {
                if (temp.Parent.FullName == target)
                {
                    return true;
                }
                else
                    temp = temp.Parent;
            }
            return false;
        }

#endif
	}

	/// <summary>
	/// DirectoryInfoExtensions
	/// </summary>
	public static class csutilsDirectoryInfoExtension
	{
#if CLR
		/// <summary>
		/// Checks if Write-Permissions for this Location are granted
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool CanWrite(this DirectoryInfo info)
		{
			try
			{
                FileInfo fi = new FileInfo(info.FullName+"\\iotest.tmp");
              
                try
                {
                    using(var fs = fi.OpenWrite())
                        fs.WriteByte((byte)32);                    
                }
                catch
                {
                    return false;
                }

                fi.Delete();
                return true;				
			}
			catch {	}

			return false;
		}
#endif
               

        /// <summary>
        /// Checks if the directory is a descendant of another directory
        /// </summary>
        /// <param name="fsi"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsDescendantOf(this DirectoryInfo fsi, DirectoryInfo dir)
        {
            if (dir == null)
                throw new ArgumentNullException("dir");

            DirectoryInfo temp = new DirectoryInfo(fsi.FullName);
            string target = dir.FullName.EndsWith("\\") ? dir.FullName.Substring(0, dir.FullName.Length - 1) : dir.FullName;

            while (temp.Parent != null)
            {
                if (temp.Parent.FullName == target)
                {
                    return true;
                }
                else
                    temp = temp.Parent;
            }
            return false;
        }
	}
}
