using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Extensions
{
	[TestFixture]
	public class ByteExtensionTest
	{
		[TestCase]
		public void TestToSHA1()
		{
			Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", new byte[]{}.ToSHA1().ToLower());
			Assert.AreEqual("c032adc1ff629c9b66f22749ad667e6beadf144b", new byte[]{88}.ToSHA1().ToLower());
		}

		[TestCase]
		public void TestToMD5()
		{
			Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", new byte[] { }.ToMD5().ToLower());
			Assert.AreEqual("02129bb861061d1a052c592e2dc6b383", new byte[] { 88 }.ToMD5().ToLower());
		}
	}
}
