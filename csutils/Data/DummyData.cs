using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Data
{
	/// <summary>
	/// Class with helper methods to create random data
	/// </summary>
	public class DummyData
	{
		/// <summary>
		/// Generates random bytes
		/// </summary>
		/// <param name="length">amount of bytes</param>
		/// <returns></returns>
		public static byte[] GenerateBytes(int length=16)
		{
			if (length < 1)
				throw new ArgumentException("length has to be > 0");

			Random r = new Random();
			byte[] buf = new byte[length];
			r.NextBytes(buf);
			return buf;
		}
	}
}
