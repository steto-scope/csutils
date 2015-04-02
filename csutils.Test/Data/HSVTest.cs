#if CLR
using System;
using System.Windows.Media;
using csutils.Data;
using NUnit.Framework;

namespace csutils.Test.Data
{
    [TestFixture]
    public class HSVTest
    {
        [TestCase]
        public void TestCreation()
        {
            byte i, j, k, l;

            i = j = k = l = 32;
            Assert.AreEqual(Color.FromArgb(i, j, k, l), HSV.FromColor(Color.FromArgb(i, j, k, l)).ToColor());
            i = j = k = l = (byte)new Random().Next(255);
            Assert.AreEqual(Color.FromArgb(i, j, k, l), HSV.FromColor(Color.FromArgb(i, j, k, l)).ToColor());
        }
    }
}
#endif