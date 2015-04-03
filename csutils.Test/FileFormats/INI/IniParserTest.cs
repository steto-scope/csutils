using System;
using csutils.FileFormats.INI;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace REC.Test.FileFormats.INI
{
    [TestFixture]
    public class IniParserTest
    {
        string t1 = "[A]\na=b";
        string t2 = "[[A]]\na=b";        
        string t4 = "[A   B]\na=b=c";
        string t5 = "a=b\n[A]\na=b";
        string t6 = "[A]\na=b\n\n\r\n[B]\na=c\n[A]\na=c";


        string tv1 = "a=b";
        string tv2 = " a   = b      ";
        string tv3 = "a=b=4";
        string tv4 = "=a";
        string tv5 = "=";
        string tv6 = "a=b ;Comment";
        string tv7 = "a=b\nb=c\r\nc=d";
        string tv8 = ";";
        

        [TestCase]
        public void TestParseValues()
        {
            List<IniSection> s;

            s = IniParser.Parse(tv1);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("b", s[0]["a"]);

            s = IniParser.Parse(tv2);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("b", s[0]["a"]);

            s = IniParser.Parse(tv3);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("b=4", s[0]["a"]);

            s = IniParser.Parse(tv4);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("a", s[0][""]);

            s = IniParser.Parse(tv5);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("", s[0][""]);

            s = IniParser.Parse(tv6);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("b ;Comment", s[0]["a"]);

            s = IniParser.Parse(tv7);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(3, s[0].Count);
            Assert.AreEqual("b", s[0]["a"]);
            Assert.AreEqual("c", s[0]["b"]);
            Assert.AreEqual("d", s[0]["c"]);

            s = IniParser.Parse(tv8);
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(0, s[0].Count);
        }

        [TestCase]
        public void TestParseMethod()
        {
            List<IniSection> s;
            
            s= IniParser.Parse("");
            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(0, s[0].Count);


        }

        [TestCase]
        public void TestParseSections()
        {
            List<IniSection> s;


            s = IniParser.Parse(t1);
            Assert.AreEqual(2, s.Count);
            Assert.AreEqual(0, s[0].Count);
            Assert.AreEqual(1, s[1].Count);
            Assert.AreEqual("A", s[1].Name);
            Assert.AreEqual("b", s[1]["a"]);

            s = IniParser.Parse(t2);
            Assert.AreEqual(2, s.Count);
            Assert.AreEqual(0, s[0].Count);
            Assert.AreEqual(1, s[1].Count);
            Assert.AreEqual("A", s[1].Name);
            Assert.AreEqual("b", s[1]["a"]);

            s = IniParser.Parse(t4);
            Assert.AreEqual(2, s.Count);
            Assert.AreEqual(0, s[0].Count);
            Assert.AreEqual(1, s[1].Count);
            Assert.AreEqual("A   B", s[1].Name);
            Assert.AreEqual("b=c", s[1]["a"]);

            s = IniParser.Parse(t5);
            Assert.AreEqual(2, s.Count);
            Assert.AreEqual(1, s[0].Count);
            Assert.AreEqual("b", s[0]["a"]);
            Assert.AreEqual(1, s[1].Count);
            Assert.AreEqual("A", s[1].Name);
            Assert.AreEqual("b", s[1]["a"]);

            s = IniParser.Parse(t6);
            Assert.AreEqual(3, s.Count);
            Assert.AreEqual(0, s[0].Count);
            Assert.AreEqual(1, s[1].Count);
            Assert.AreEqual("A", s[1].Name);
            Assert.AreEqual("c", s[1]["a"]);
            Assert.AreEqual(1, s[2].Count);
            Assert.AreEqual("B", s[2].Name);
            Assert.AreEqual("c", s[1]["a"]);
        }
    }
}
