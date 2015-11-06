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
using System.Threading.Tasks;

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
	}
}
