using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public int CalculateTheAnswerOfTheUltimateQuestionOfLifeTheUniverseAndEverything()
        {
            Thread.Sleep(int.MaxValue);
            return 42; 
        }

        [TestCase]
        public void TestXor()
        {
            byte[] a1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            byte[] a2 = new byte[]{0,0,0,0,0,0,0,0};
            byte[] a3 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
            byte[] a4 = new byte[] { 255, 254, 253, 252, 251, 250, 249, 248 };
            byte[] a5 = new byte[] { 42 };

            Assert.AreEqual(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, a1.Xor(a2, 0, 0, a1.Length));
            Assert.AreEqual(new byte[] { 255, 254, 253, 252, 251, 250, 249, 248 }, a1.Xor(a3, 0, 0, a1.Length));
            Assert.AreEqual(new byte[] { 0, 254, 253, 252, 251, 250, 249, 248 }, a1.Xor(a3, 1, 0, a1.Length));
            Assert.AreEqual(new byte[] { 255, 254, 253, 252, 251, 250, 6, 7 }, a1.Xor(a3, 0, 2, a1.Length));
            Assert.AreEqual(new byte[] { 253, 253, 249, 249, 253, 253, 6, 7 }, a1.Xor(a4, 0, 2, a1.Length));
            Assert.AreEqual(a1, a1.Xor(new byte[] { }, 99, 99, a1.Length));
            Assert.AreEqual(a5, a5.Xor(a1, 0, 0, a1.Length));
            Assert.AreEqual(new byte[] { 46 }, a5.Xor(a1, 0, 4, a1.Length));
            Assert.AreEqual(new byte[] { 46 }, a5.Xor(a1, 0, 4, 666));

            try
            {
                a1.Xor(a2, -1, 0, a1.Length);
                Assert.Fail();
            }
            catch { }
            try
            {
                a1.Xor(a2, 0, -1, a1.Length);
                Assert.Fail();
            }
            catch { }
            try
            {
                a1.Xor(a2, 99, 0, a1.Length);
                Assert.Fail();
            }
            catch { }
            try
            {
                a1.Xor(a2, 0, 99, a1.Length);
                Assert.Fail();
            }
            catch { }
        }	

	}
}
