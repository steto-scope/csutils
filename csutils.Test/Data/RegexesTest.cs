using System;
using csutils.Data;
using System.Text;
using NUnit.Framework;

namespace csutils.Test.Data
{
    [TestFixture]
    public class RegexesTest
    {
        public static string GenerateUnicodeString(int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=start; i<end; i++)
            {
                sb.Append((char)i);
            }
            return sb.ToString();
        }

        [TestCase]
        public void TestXMLTags()
        {
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml />"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml/>"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml prop />"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml prop=4 />"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml\n prop=\n4 />\n"));
            Assert.IsTrue(Regexes.XMLTagRegex.Matches("<xml prop=\"test>\" /> <xml prop=4 />").Count==2);
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml prop='xml' >"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch("<xml>"));
            Assert.IsTrue(Regexes.XMLTagRegex.IsMatch(" < xml > "));
        }

        [TestCase]
        public void TestInvalidUriCharactersRegex()
        {
            Assert.IsFalse(Regexes.InvalidUriCharacterRegex.IsMatch("https://user:pass@www.server.net:8080/res/./../page.php?p=value&p2=value"));
            Assert.IsTrue(Regexes.InvalidUriCharacterRegex.IsMatch("https://user:pass@www.server.net:8080/res/./../page.php?p=value&p²=value"));
        }
         [TestCase]
        public void TestNonASCIIRegex()
        {
            Assert.IsFalse(Regexes.NonASCIICharacterRegex.IsMatch(GenerateUnicodeString(32,255)));
            Assert.IsTrue(Regexes.NonASCIICharacterRegex.IsMatch(GenerateUnicodeString(32,257)));
        }
    }
}
