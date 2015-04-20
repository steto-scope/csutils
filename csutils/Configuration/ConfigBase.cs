using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csutils.Configuration
{
	
	public abstract class ConfigBase
	{
		private string userconfigfile;

		public string UserConfigFile
		{
			get { return UserConfigPath + "\\user.config"; }
		}

		private string appconfigfile;

		public string AppConfigFile
		{
			get { return AppConfigPath + "\\app.config"; }
		}

		private string userconfigpath;

		public string UserConfigPath
		{
			get 
			{
				if (string.IsNullOrEmpty(userconfigpath))
					return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;

				try
				{
					Path.GetFullPath(userconfigfile); //invalid paths will cause an exception
				}
				catch
				{
					return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;
				}

				return userconfigpath;
			}
			set
			{

				while (value.EndsWith(@"\"))
					value = value.Substring(0, value.Length - 1);

				userconfigpath = value;
			}
		}

		private string appconfigpath;

		public string AppConfigPath
		{
			get
			{
				if (string.IsNullOrEmpty(appconfigpath))
					return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;

				try
				{
					Path.GetFullPath(appconfigfile);
				}
				catch
				{
					return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;
				}

				return appconfigpath;
			}
			set
			{

				while (value.EndsWith(@"\"))
					value = value.Substring(0, value.Length - 1);

				appconfigpath = value;
			}
		}

		
		private SerializableDictionary<string, object> userconfig;
		private SerializableDictionary<string, object> appconfig;
		private SerializableDictionary<string, object> instconfig;

		public T Clone<T>() where T : ConfigBase, new()
		{
			T t = new T();

			foreach(var s in userconfig.Keys)
				t.Set(s,userconfig[s],ConfigLevel.User);
			foreach (var s in appconfig.Keys)
				t.Set(s, appconfig[s], ConfigLevel.Application);
			foreach (var s in instconfig.Keys)
				t.Set(s, instconfig[s], ConfigLevel.Instance);

			return t;
		}

		public ConfigBase()
		{
			userconfig = new SerializableDictionary<string, object>();
			instconfig = new SerializableDictionary<string, object>();
			appconfig = new SerializableDictionary<string, object>();

			FillDefaults();
		}


		public void LoadUserConfig()
		{
			try
			{
				using (FileStream fs = new FileStream(UserConfigFile, FileMode.Open))
				{
					XmlSerializer x = new XmlSerializer(typeof(SerializableDictionary<string, object>));
					var d = (SerializableDictionary<string, object>)x.Deserialize(fs);
					foreach (var k in d.Keys)
					{
						userconfig[k] = d[k];
					}
				}
			}
			catch
			{

			}
		}

		public void LoadAppConfig()
		{
			try
			{
				using (FileStream fs = new FileStream(AppConfigFile, FileMode.Open))
				{
					XmlSerializer x = new XmlSerializer(typeof(SerializableDictionary<string, object>));
					var d = (SerializableDictionary<string, object>)x.Deserialize(fs);
					foreach (var k in d.Keys)
					{
						appconfig[k] = d[k];
					}
				}
			}
			catch
			{

			}
		}

		public void LoadAll()
		{
			LoadAppConfig();
			LoadUserConfig();
		}


		public void SaveUserConfig()
		{
			if (!Directory.Exists(UserConfigPath))
				Directory.CreateDirectory(UserConfigPath);

			using (FileStream fs = new FileStream(UserConfigFile, FileMode.Create))
			{
				XmlSerializer x = new XmlSerializer(typeof(SerializableDictionary<string, object>));
				x.Serialize(fs,userconfig);
			}
		}

		public void SaveAppConfig()
		{
			if (!Directory.Exists(AppConfigPath))
				Directory.CreateDirectory(AppConfigPath);

			using (FileStream fs = new FileStream(AppConfigFile, FileMode.Create))
			{
				XmlSerializer x = new XmlSerializer(typeof(SerializableDictionary<string, object>));
				x.Serialize(fs, appconfig);
			}
		}

		public void SaveAll()
		{
			SaveAppConfig();
			SaveUserConfig();
		}

		protected void Set(string key, object o, ConfigLevel level = ConfigLevel.User)
		{
			switch(level)
			{
				case ConfigLevel.Application:
					appconfig[key] = o;
					break;
				case ConfigLevel.User:
					userconfig[key] = o;
					break;
				case ConfigLevel.Instance:
					instconfig[key] = o;
					break;
			}
		}

		protected T Get<T>(string key, ConfigLevel level = ConfigLevel.User)
		{
			switch (level)
			{
				case ConfigLevel.Application:
						if(appconfig.ContainsKey(key))
							return (T)appconfig[key];
						return default(T);
				case ConfigLevel.User:
						if (userconfig.ContainsKey(key))
							return (T)userconfig[key];
						return default(T);
				case ConfigLevel.Instance:
				default:
						if (instconfig.ContainsKey(key))
							return (T)instconfig[key];
						return default(T);
			}
		}

		public abstract void FillDefaults();
	}
}
