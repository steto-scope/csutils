using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace csutils.Data
{
    /// <summary>
    /// Xml-Serializable version of the .NET dictionary class
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <inheritdoc />
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public SerializableDictionary() : base()
        {

        }
        /// <inheritdoc />
        public SerializableDictionary(int capacity) : base(capacity)
        {

        }
        /// <inheritdoc />
        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {

        }
        /// <inheritdoc />
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
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
                TKey key;
                if (reader.Name == "Item")
                {
                    reader.Read();
                    Type keytype = Type.GetType(reader.GetAttribute("type"));
                    if (keytype != null)
                    {
                        reader.Read();
                        key = (TKey)new XmlSerializer(keytype).Deserialize(reader);
                        reader.ReadEndElement();
                        Type valuetype = (reader.HasAttributes) ? Type.GetType(reader.GetAttribute("type")) : null;
                        if (valuetype != null)
                        {
                            reader.Read();
                            Add(key, (TValue)new XmlSerializer(valuetype).Deserialize(reader));
                            reader.ReadEndElement();
                        }
                        else
                        {
                            Add(key, default(TValue));
                            reader.Skip();
                        }
                    }
                    reader.ReadEndElement();
                    reader.MoveToContent();
                }
            }
            reader.ReadEndElement();
        }
        /// <inheritdoc />
        public virtual void WriteXml(XmlWriter writer)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                TKey key = Keys.ElementAt(i);
                TValue value = this.ElementAt(i).Value;

                writer.WriteStartElement("Item");
                writer.WriteStartElement("Key");
                writer.WriteAttributeString(string.Empty, "type", string.Empty, key.GetType().AssemblyQualifiedName);
                new XmlSerializer(key.GetType()).Serialize(writer, key);      
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                if (value != null)
                {
                    writer.WriteAttributeString(string.Empty, "type", string.Empty, value.GetType().AssemblyQualifiedName);
                    new XmlSerializer(value.GetType()).Serialize(writer, value);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }     
    }
}
