using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Performs xor operations on byte arrays. If length exceeds the array size, the longest possible length will be used
        /// </summary>
        /// <param name="b"></param>
        /// <param name="secondArray">byte array to be performed with</param>
        /// <param name="firstIndex">start index of the source array</param>
        /// <param name="secondIndex">start index of the second array</param>
        /// <param name="length">length</param>
        /// <returns>array of transformed bytes</returns>
        public static byte[] Xor(this byte[] b, byte[] secondArray, int firstIndex, int secondIndex, int length)
        {
            if (secondArray == null || secondArray.Length<1)
                return b;

            if (firstIndex >= b.Length)
                throw new ArgumentException("first index out of range");
            if (secondIndex >= secondArray.Length)
                throw new ArgumentException("second index out of range");
            if (length < 0)
                throw new ArgumentException("length has to be positive");
            

            int srcMaxLength = Math.Min(b.Length - firstIndex, length);
            int tarMaxLength = Math.Min(secondArray.Length - secondIndex, length);
            int reallength = Math.Min(srcMaxLength, tarMaxLength);            

            byte[] res = new byte[b.Length];
            Array.Copy(b, res, firstIndex);
            for(int i=0; i<reallength; i++)
            {
                res[i + firstIndex] = (byte)(b[i + firstIndex] ^ secondArray[i + secondIndex]);
            }
            Array.Copy(b, firstIndex + reallength, res, firstIndex + reallength, b.Length - (firstIndex + reallength));
            return res;
        }

        /// <summary>
        /// Performs xor operations on byte arrays. 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="secondArray">byte array to be performed with</param>
        /// <returns></returns>
        public static byte[] Xor(this byte[] b, byte[] secondArray)
        {
            return b.Xor(secondArray, 0, 0, secondArray.Length);
        }

	}
}
