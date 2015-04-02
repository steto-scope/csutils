using System;
using csutils.Globalisation;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace REC.Test.Globalisation
{
    [TestFixture]
    public class TranslationManagerTest
    {
        [SetUp]
        public void Install()
        {
            string de = "<root><e1>t1</e1><e2>t2</e2><e3>t3</e3><e4>t4</e4></root>";
            string de_name = "<root><e1>d1</e1><e2>d2</e2><e3>d3</e3><e4>d4</e4><e5>d5</e5></root>";
            string en = "<root><e1>en t1</e1><e2>en t2</e2><e3>t3</e3><e6>en t6</e6></root>";

            TranslationManager.CurrentCulture = new System.Globalization.CultureInfo("de");

            TranslationManager.Add(de.ToStream(), ".xml", new System.Globalization.CultureInfo("de"));
            TranslationManager.Add(de_name.ToStream(), ".xml", new System.Globalization.CultureInfo("de"),"name");
            TranslationManager.Add(en.ToStream(), ".xml", new System.Globalization.CultureInfo("en"));

        }

        [TestCase]
        public void TestAdd()
        {
            Assert.AreEqual(2, TranslationManager.InstalledTranslations().ToList().Count);
            Assert.AreEqual(1, TranslationManager.InstalledTranslations("name").ToList().Count);
            Assert.AreEqual(0, TranslationManager.InstalledTranslations("another name").ToList().Count);
        }

        [TestCase]
        public void TestAddFile()
        {
            File.WriteAllText("de-de.es-ES.xml", "<root><e1>Spanish!</e1></root>", Encoding.Unicode);
            
            TranslationManager.Add("de-de.es-ES.xml");            
            Assert.AreEqual(3, TranslationManager.InstalledTranslations().ToList().Count);
            Assert.AreEqual("Spanish!",TranslationManager.Translate("e1","",new System.Globalization.CultureInfo("es")));
                        
            File.WriteAllText("de-de.es.xml", "<root><e1>Overwrite!</e1></root>", Encoding.Unicode);
            TranslationManager.Add("de-de.es.xml");
            Assert.AreEqual(3, TranslationManager.InstalledTranslations().ToList().Count);
            Assert.AreEqual("Overwrite!", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("es")));

            File.WriteAllText("de-DE.e.xml", "<root><e1>DE!</e1></root>", Encoding.Unicode);
            TranslationManager.Add("de-DE.e.xml");
            Assert.AreEqual(3, TranslationManager.InstalledTranslations().ToList().Count);
            Assert.AreEqual("Overwrite!", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("es")));
            Assert.AreEqual("DE!", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("de")));

            File.WriteAllText("es.xml", "<root><e1>Overwrite 2!</e1></root>", Encoding.Unicode);
            TranslationManager.Add("es.xml");
            Assert.AreEqual(3, TranslationManager.InstalledTranslations().ToList().Count);
            Assert.AreEqual("Overwrite 2!", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("es")));

            try
            {
                File.WriteAllText("de-de.e.xml", "<root><e1>DE!</e1></root>", Encoding.Unicode);
                TranslationManager.Add("de-de.e.xml");
                Assert.Fail();
            }
            catch
            {

            }

            File.Delete("de-de.es-ES.xml");
            File.Delete("de-de.es.xml");
            File.Delete("de-de.e.xml");
            File.Delete("de-DE.e.xml");
            File.Delete("es.xml");
        }

        [TestCase]
        public void TestTranslate()
        {
       
 
            Assert.AreEqual("t1", TranslationManager.Translate("e1"));
            Assert.AreEqual("d1", TranslationManager.Translate("e1", "name"));
            Assert.AreEqual("d1", TranslationManager.Translate("e1", "name", new System.Globalization.CultureInfo("de")));
            Assert.AreEqual("en t1", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("en")));
            Assert.AreEqual("e1", TranslationManager.Translate("e1", "name", new System.Globalization.CultureInfo("en")));

            Assert.AreEqual("en t6", TranslationManager.Translate("e6"));
            Assert.AreEqual("e6", TranslationManager.Translate("e6","name"));
            Assert.AreEqual("en t6", TranslationManager.Translate("e6", "", new System.Globalization.CultureInfo("en")));
            Assert.AreEqual("e6", TranslationManager.Translate("e6", "name", new System.Globalization.CultureInfo("en")));

            Assert.IsNull(TranslationManager.Translate(null));

            //overwrite test
            string de = "<root><e1>x1</e1><e2>x2</e2><e3>x3</e3><e4>x4</e4></root>";
            TranslationManager.Add(de.ToStream(), ".xml", new System.Globalization.CultureInfo("de"));
            Assert.AreEqual("x1", TranslationManager.Translate("e1"));
            Assert.AreEqual("d1", TranslationManager.Translate("e1", "name"));
            Assert.AreEqual("en t1", TranslationManager.Translate("e1", "", new System.Globalization.CultureInfo("en")));
            Assert.AreEqual("e1", TranslationManager.Translate("e1", "name", new System.Globalization.CultureInfo("en")));

            bool received = false;
            TranslationManager.LanguageChanged += delegate { received = true; };
            Assert.IsFalse(received);
            Assert.AreEqual("x1", TranslationManager.Translate("e1"));
            TranslationManager.CurrentCulture = new System.Globalization.CultureInfo("en");
            Assert.IsTrue(received);
            Assert.AreEqual("en t1", TranslationManager.Translate("e1"));

        }


        [TearDown]
        public void Uninstall()
        {
            TranslationManager.Clear();
        }
    }
}
