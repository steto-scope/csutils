using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary>
	/// Extensions for Bytes
	/// </summary>
	public static class csutilsByteExtension
	{
		/// <summary>
		/// Calculates the SHA1-Hash of the byte-Array
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToSHA1(this byte[] bytes)
		{
			string hex;
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				hex = BitConverter.ToString(sha1.ComputeHash(bytes)).Replace("-", string.Empty);
			}
			return hex;
		}

		/// <summary>
		/// Calculates the MD5-Hash of the byte-Array
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToMD5(this byte[] bytes)
		{
			string hex;
			using (MD5 md5 = new MD5CryptoServiceProvider())
			{
				hex = BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", string.Empty);
			}
			return hex;
		}
	}
}
