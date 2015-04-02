using System;
using System.Xml.Serialization;
using csutils.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using csutils.Test;

namespace REC.Test.Data
{
    [TestFixture]
    public class SerializableDictionaryTest
    {
       
        [TestCase]
        public void TestXmlSerialisation()
        {
            SubBase sb = new SubBase() { ValueType = 42, ImmutableType="Hello World" };

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Base), new Type[] { typeof(SubBase) });
                MemoryStream ms = new MemoryStream();
                s.Serialize(ms, sb);
                
                ms.Seek(0, SeekOrigin.Begin);
                
                XmlSerializer s2 = new XmlSerializer(typeof(Base), new Type[] { typeof(SubBase) });
                SubBase sb2 = (SubBase)s2.Deserialize(ms);

                Assert.AreEqual(sb.ImmutableType, sb2.ImmutableType);
                Assert.AreEqual(sb.ValueType, sb2.ValueType);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestCase]
        public void TestDictionaryXmlSerialisation()
        {
            SerializableDictionary<string, string> d1 = new SerializableDictionary<string, string>();
            d1["Hello"] = "World";

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                MemoryStream ms = new MemoryStream();
                s.Serialize(ms, d1);

                ms.Seek(0, SeekOrigin.Begin);

                XmlSerializer s2 = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                SerializableDictionary<string, string> d1e = (SerializableDictionary<string, string>)s2.Deserialize(ms);

                Assert.IsTrue(d1e.ContainsKey("Hello"));
                Assert.AreEqual(d1e["Hello"], "World");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }


            SerializableDictionary<int,NonSerializableClass> d2 = new SerializableDictionary<int, NonSerializableClass>();
            d2[42] = new NonSerializableClass() { Prop = "Truth" };

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(SerializableDictionary<int, NonSerializableClass>));
                MemoryStream ms = new MemoryStream();
                s.Serialize(ms, d2);

                ms.Seek(0, SeekOrigin.Begin);

                XmlSerializer s2 = new XmlSerializer(typeof(SerializableDictionary<int, NonSerializableClass>));
                SerializableDictionary<int, NonSerializableClass> d2e = (SerializableDictionary<int, NonSerializableClass>)s2.Deserialize(ms);

                Assert.IsTrue(d2e.ContainsKey(42));
                Assert.AreEqual(d2e[42],d2[42]);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                
            }
        }

        class NonSerializableClass
        {
            public string Prop { get; set; }
        }

    }
}
