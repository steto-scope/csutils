using System;
using csutils.FileFormats.INI;
using System.Collections.Generic;
using NUnit.Framework;

namespace REC.Test.FileFormats.INI
{
    [TestFixture]
    public class IniSectionTest
    {
        [TestCase]
        public void TestSection()
        {
            IniSection s = new IniSection();
            Assert.AreEqual("", s.Name);
            Assert.AreEqual(0, s.Count);
            Assert.AreEqual(0, s.Keys.Length);

            s["a"] = "b";
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s.Keys.Length);
            Assert.AreEqual("b", s["a"]);

            s["b"] = null;
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s.Keys.Length);
            Assert.AreEqual("b", s["a"]);

            s["a"] = null;
            Assert.AreEqual(0, s.Count);
            Assert.AreEqual(0, s.Keys.Length);
        }

        [TestCase]
        public void TestToString()
        {
            IniSection s = new IniSection();

            s["a"] = "b";
            Assert.AreEqual("a=b", s.ToString().Trim());

            s.Name="abc";
            Assert.AreEqual("[abc]\r\na=b", s.ToString().Trim());
        }
    }
}
