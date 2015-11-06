using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace csutils.Data
{
	/// <summary>
	/// Simplified Version of SerializableDictionary for use with string keys
	/// </summary>
	/// <typeparam name="TValue">value type</typeparam>
	public class SerializableStringDictionary<TValue> : Dictionary<string, TValue>, IXmlSerializable
	{
		/// <inheritdoc />
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		/// <inheritdoc />
		public SerializableStringDictionary()
			: base()
		{

		}
		/// <inheritdoc />
		public SerializableStringDictionary(int capacity)
			: base(capacity)
		{

		}
		/// <inheritdoc />
		public SerializableStringDictionary(IDictionary<string, TValue> dictionary)
			: base(dictionary)
		{

		}
		/// <inheritdoc />
		public virtual void ReadXml(XmlReader reader)
		{
			Boolean wasEmpty = reader.IsEmptyElement;
			reader.Read();
			if (wasEmpty)
				return;

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				string key = reader.Name;
				Type valuetype = (reader.HasAttributes) ? Type.GetType(reader.GetAttribute("type")) : null;
				reader.Read();										
				
				if (valuetype != null)
				{
					//reader.Read();
					Add(key, (TValue)new XmlSerializer(valuetype).Deserialize(reader));
					//reader.ReadEndElement();
				}
				else
				{
					Add(key, default(TValue));
				//	reader.Skip();
				}
				reader.ReadEndElement();
				reader.MoveToContent();				
			}
			reader.ReadEndElement();
		}
		/// <inheritdoc />
		public virtual void WriteXml(XmlWriter writer)
		{
			for (int i = 0; i < Keys.Count; i++)
			{
				string key = Keys.ElementAt(i);
				TValue value = this.ElementAt(i).Value;

				writer.WriteStartElement(key);
				
				if (value != null)
				{
					writer.WriteAttributeString(string.Empty, "type", string.Empty, value.GetType().AssemblyQualifiedName);
					new XmlSerializer(value.GetType()).Serialize(writer, value);
				}
				writer.WriteEndElement();
			}
		}
	}
}
