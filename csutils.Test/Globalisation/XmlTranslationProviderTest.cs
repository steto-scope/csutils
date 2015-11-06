using System;
using csutils.Globalisation.TranslationProvider;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace csutils.Test.Globalisation
{
    [TestFixture]
    public class XmlTranslationProviderTest
    {
        [TestCase]
        public void TestGetContent()
        {

            XmlTranslationProvider p = new XmlTranslationProvider();
            var dict = p.Parse("<?xml version=\"1.0\" ?><root> <e1>t1</e1> <e2 value=\"t2\" /> <e3 test=\"t3\" />  </root>");

            Assert.IsTrue(dict.ContainsKey("e1"));
            Assert.IsTrue(dict.ContainsKey("e2"));
            Assert.IsFalse(dict.ContainsKey("e3"));
            Assert.AreEqual("t1", dict["e1"]);
            Assert.AreEqual("t2", dict["e2"]);
            
            XmlTranslationProvider p2 = new XmlTranslationProvider();
            p2.ValueAttributeName = "test";
            dict = p2.Parse("<?xml version=\"1.0\" ?><root> <e1>t1</e1> <e2 value=\"t2\" />    <e3 test=\"t3\" /><e3>t4</e3> </root>");

            Assert.IsTrue(dict.ContainsKey("e1"));
            Assert.IsFalse(dict.ContainsKey("e2"));
            Assert.IsTrue(dict.ContainsKey("e3"));
            Assert.AreEqual("t1", dict["e1"]);
            Assert.AreEqual("t4", dict["e3"]);

            try
            {
                XmlTranslationProvider p3 = new XmlTranslationProvider();
                dict = p3.Parse("<?xml version\"1.0\" ?><root> <e1>t1</e1> </root>");
                Assert.Fail();
            }
            catch { }


        }
    }
}
