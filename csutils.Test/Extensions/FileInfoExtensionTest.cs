using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Extensions
{
    [TestFixture]
    public class FileInfoExtensionTest
    {
#if CLR
        [TestCase]
        public void TestCanWrite()
        {
            FileInfo fi = new FileInfo("test.txt");
            Assert.IsTrue(fi.CanWrite());


            Assert.IsFalse(new FileInfo("C:\\Windows\\System32\\sethc.exe").CanWrite());
            try
            {
                new FileInfo("C:\\Windows\\System32?\\sethc.exe").CanWrite();
                Assert.Fail();
            }
            catch { }
        }

        [TestCase]
        public void TestToSHA1()
        {
            File.Create("sha1.tmp").Close();
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", new FileInfo("sha1.tmp").ToSHA1().ToLower());
            File.WriteAllBytes("sha1.tmp", new byte[] { 88 });
            Assert.AreEqual("c032adc1ff629c9b66f22749ad667e6beadf144b", new FileInfo("sha1.tmp").ToSHA1().ToLower());
            File.Delete("sha1.tmp");
        }

        [TestCase]
        public void TestToMD5()
        {
            File.Create("md5.tmp").Close();
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", new FileInfo("md5.tmp").ToMD5().ToLower());
            File.WriteAllBytes("md5.tmp", new byte[] { 88 });
            Assert.AreEqual("02129bb861061d1a052c592e2dc6b383", new FileInfo("md5.tmp").ToMD5().ToLower());
            File.Delete("md5.tmp");
        }
#endif

        [TestCase]
        public void TestIsDescendantOf()
        {
            DirectoryInfo d1 = new DirectoryInfo("C:\\abc\\def");
            DirectoryInfo d2 = new DirectoryInfo("C:\\abc");
            DirectoryInfo d3 = new DirectoryInfo("C:\\abc\\def\\ghi");
            DirectoryInfo d4 = new DirectoryInfo("C:\\abc\\");
            DirectoryInfo d5 = new DirectoryInfo("D:\\abc\\def");

            FileInfo f1 = new FileInfo("C:\\abc\\def\\file.ext");

            Assert.IsTrue(f1.IsDescendantOf(d1));
            Assert.IsTrue(f1.IsDescendantOf(d2));
            Assert.IsTrue(f1.IsDescendantOf(d4));
            Assert.IsFalse(f1.IsDescendantOf(d3));
            Assert.IsFalse(f1.IsDescendantOf(d5));
            try { f1.IsDescendantOf(null); Assert.Fail(); } catch { }


            Assert.IsTrue(d1.IsDescendantOf(d2));
            Assert.IsFalse(d1.IsDescendantOf(d3));
            Assert.IsTrue(d1.IsDescendantOf(d4));
            Assert.IsFalse(d1.IsDescendantOf(d5));
            Assert.IsFalse(d1.IsDescendantOf(d1));
            try { d1.IsDescendantOf(null); Assert.Fail(); } catch { }


        }
    }
}
