using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csutils.Configuration
{
	
	public abstract class ConfigBase
	{
		private TwoKeyDictionary<string,ConfigLevel,object> dict;
		[XmlElement("Settings")]
		public TwoKeyDictionary<string,ConfigLevel,object> Dictionary
		{
			get { return dict; }
			set { dict = value; }
		}

		public T Clone<T>() where T : ConfigBase, new()
		{
			T t = new T();
			foreach(var s in dict.Keys)
			{
				t.Set(s.Item1,dict[s],s.Item2);
			}
			return t;
		}

		public ConfigBase()
		{
			dict = new TwoKeyDictionary<string, ConfigLevel, object>();
			FillDefaults();
		}

		public ConfigBase(string file)
		{
			FillDefaults();
			Load(file);
		}

		public void Load(string file)
		{
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				XmlSerializer x = new XmlSerializer(typeof(TwoKeyDictionary<string, ConfigLevel, object>));
				var d = (TwoKeyDictionary<string, ConfigLevel, object>)x.Deserialize(fs);
				foreach (var k in d.Keys)
				{
					dict[k.Item1, k.Item2] = d[k.Item1, k.Item2];
				}
			}
		}

		public void Save(string file)
		{
			using (FileStream fs = new FileStream(file, FileMode.Create))
			{
				XmlSerializer x = new XmlSerializer(typeof(TwoKeyDictionary<string, ConfigLevel, object>));
				x.Serialize(fs,dict);
			}
		}

		protected void Set(string key, object o, ConfigLevel level = ConfigLevel.User)
		{
			dict[key,level] = o;
		}

		protected T Get<T>(string key, ConfigLevel level = ConfigLevel.User)
		{
			return (T)dict[key,level];
		}

		public abstract void FillDefaults();
	}
}
