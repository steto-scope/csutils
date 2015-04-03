using System;
using csutils.FileFormats.INI;
using System.IO;
using NUnit.Framework;

namespace REC.Test.FileFormats.INI
{
    [TestFixture]
    public class IniFileTest
    {
        [TestCase]
        public void TestLoad()
        {
            string f1 = "a=b\r\n [A]\r\n b=c\r\n c=d\r\n [B]\r\n d=e";
            string f2 = "a=x\r\n [A]\r\n b=y\r\n z=Z\r\n [C]\r\n d=e";

            IniFile if1 = new IniFile(f1);
            Assert.AreEqual(3, if1.SectionCount);
            Assert.AreEqual(2, if1["A"].Count);
            Assert.AreEqual(1, if1["B"].Count);
            Assert.AreEqual("b", if1["", "a"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("e", if1["B", "d"]);

            if1.Load(f2, IniFileLoadStrategy.Replace);
            Assert.AreEqual(3, if1.SectionCount);
            Assert.AreEqual(2, if1["A"].Count);
            Assert.AreEqual(1, if1["C"].Count);
            Assert.IsNull(if1["A", "c"]);
            Assert.AreEqual("x", if1["", "a"]);
            Assert.AreEqual("y", if1["A", "b"]);
            Assert.AreEqual("Z", if1["A", "z"]);
            Assert.AreEqual("e", if1["C", "d"]);


            if1 = new IniFile(f1);
            if1.Load(f2, IniFileLoadStrategy.Add);
            Assert.AreEqual(4, if1.SectionCount);
            Assert.AreEqual(3, if1["A"].Count);
            Assert.AreEqual(1, if1["B"].Count);
            Assert.AreEqual(1, if1["C"].Count);
            Assert.AreEqual("b", if1["", "a"]);
            Assert.AreEqual("Z", if1["A", "z"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("e", if1["B", "d"]);
            Assert.AreEqual("e", if1["C", "d"]);



            if1 = new IniFile(f1);
            if1.Load(f2, IniFileLoadStrategy.Update);
            Assert.AreEqual(3, if1.SectionCount);
            Assert.AreEqual(2, if1["A"].Count);
            Assert.AreEqual(1, if1["B"].Count);
            Assert.AreEqual("x", if1["", "a"]);
            Assert.IsNull(if1["A", "z"]);
            Assert.AreEqual("y", if1["A", "b"]);
            Assert.AreEqual("d", if1["A", "c"]);
            Assert.AreEqual("e", if1["B", "d"]);


            if1 = new IniFile(f1);
            if1.Load(f2, IniFileLoadStrategy.Merge);
            Assert.AreEqual(4, if1.SectionCount);
            Assert.AreEqual(3, if1["A"].Count);
            Assert.AreEqual(1, if1["B"].Count);
            Assert.AreEqual(1, if1["C"].Count);
            Assert.AreEqual("x", if1["", "a"]);
            Assert.AreEqual("Z", if1["A", "z"]);
            Assert.AreEqual("y", if1["A", "b"]);
            Assert.AreEqual("d", if1["A", "c"]);
            Assert.AreEqual("e", if1["B", "d"]);
            Assert.AreEqual("e", if1["C", "d"]);
        }

        [TestCase]
        public void TestSaveLoad()
        {
            string f1 = "a=b\r\n [A]\r\n b=c\r\n c=d\r\n [B]\r\n d=e";

            IniFile if1 = new IniFile(f1);
            Assert.AreEqual(3, if1.SectionCount);
            Assert.AreEqual(2, if1["A"].Count);
            Assert.AreEqual(1, if1["B"].Count);
            Assert.AreEqual("b", if1["", "a"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("c", if1["A", "b"]);
            Assert.AreEqual("e", if1["B", "d"]);

            if1.Save("tmp.bak");


            IniFile if2 = new IniFile(File.OpenRead("tmp.bak"));
            Assert.AreEqual(3, if2.SectionCount);
            Assert.AreEqual(2, if2["A"].Count);
            Assert.AreEqual(1, if2["B"].Count);
            Assert.AreEqual("b", if2["", "a"]);
            Assert.AreEqual("c", if2["A", "b"]);
            Assert.AreEqual("c", if2["A", "b"]);
            Assert.AreEqual("e", if2["B", "d"]);

            File.Delete("tmp.bak");
        }

        [TestCase]
        public void TestAccess()
        {
            string f1 = "a=b\r\n [A]\r\n b=c\r\n c=d\r\n [B]\r\n d=e";
            IniFile if1 = new IniFile(f1);

            Assert.IsNull(if1["C"]);
            Assert.IsNull(if1["A","d"]);
            Assert.IsNull(if1["",""]);
            Assert.IsNull(if1[null]);
            Assert.IsNull(if1[null,null]);

            Assert.AreEqual("b", if1["", "a"]);


        }
    }
}
