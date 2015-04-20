using csutils.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csutils.Test.Data
{
	[TestFixture]
	public class SerializableStringDictionary
	{

		[TestCase]
		public void TestXmlSerialisation()
		{
			SerializableStringDictionary<int> id = new SerializableStringDictionary<int>();
			id["Test"] = 42;
			id["Test2"] = 43;


			try
			{
				XmlSerializer s = new XmlSerializer(typeof(SerializableStringDictionary<int>));
				MemoryStream ms = new MemoryStream();
				s.Serialize(ms, id);

				ms.Seek(0, SeekOrigin.Begin);

				XmlSerializer s2 = new XmlSerializer(typeof(SerializableStringDictionary<int>));
				SerializableStringDictionary<int> d2e = (SerializableStringDictionary<int>)s2.Deserialize(ms);

				Assert.IsTrue(d2e.ContainsKey("Test"));
				Assert.AreEqual(d2e["Test"], id["Test"]);
				Assert.AreEqual(d2e["Test2"], id["Test2"]);
				
			}
			catch
			{
			}



			SerializableStringDictionary<NonSerializableClass> id2 = new SerializableStringDictionary<NonSerializableClass>();
			id2["Test"] = new NonSerializableClass() { Prop = "Foo" };


			try
			{
				XmlSerializer s = new XmlSerializer(typeof(SerializableStringDictionary<NonSerializableClass>),new Type[]{typeof(NonSerializableClass)});
				MemoryStream ms = new MemoryStream();
				s.Serialize(ms, id2);

				ms.Seek(0, SeekOrigin.Begin);

				XmlSerializer s2 = new XmlSerializer(typeof(SerializableStringDictionary<NonSerializableClass>),new Type[]{typeof(NonSerializableClass)});
				SerializableStringDictionary<NonSerializableClass> d2e = (SerializableStringDictionary<NonSerializableClass>)s2.Deserialize(ms);

				Assert.IsTrue(d2e.ContainsKey("Test"));
				Assert.AreEqual(d2e["Test"].Prop, id2["Test"].Prop);
			}
			catch
			{

			}

		}

	}
	public class NonSerializableClass
	{
		public string Prop { get; set; }
	}
}
