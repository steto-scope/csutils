using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace csutils.Test.Extensions
{
    [TestFixture]
    public class StreamExtensionsTest
    {
        [TestCase]
        public void TestToString()
        {
            Stream s = "Test".ToStream();

            Assert.AreEqual("Test", s.ToString(Encoding.Unicode));
            Assert.AreEqual("Test", s.ToString(Encoding.Unicode));
        }
    }
}
