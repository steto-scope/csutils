using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace csutils.Configuration
{
	/// <summary>
	/// BaseClass for all Configuration Files. 
	/// 
	/// Derive from this class and add custom properties. The getter and setter of the strong typed Properties 
	/// should internally call Get() and Set() for using the mechanics of this class. 
	/// Usage for Configuration-Windows: Use Clone() to create a copy of the config object and use this to present and 
	/// change settings by the user. After click on "Apply" use Merge() to apply the changed settings to the active 
	/// object. Alternatively it is possible to directly change the settings
	/// 
	/// IMPORTANT: 
	/// BaseConfig uses XmlSerializer for Serialisation which automatically gets the type using reflection. 
	/// Be sure that your Property-Type is serializable. Due to its use of Reflection serialisation is rather
	/// slow. 
	/// </summary>
	public abstract class ConfigBase
	{
		/// <summary>
		/// Gets the file containing the User-Level settings
		/// </summary>
		public string UserConfigFile
		{
			get { return UserConfigPath + "\\user.config"; }
		}

		/// <summary>
		/// Gets the file containing the Application-Level settings
		/// </summary>
		public string AppConfigFile
		{
			get { return AppConfigPath + "\\app.config"; }
		}

		private string userconfigpath;
		/// <summary>
		/// Gets or sets the path to the User-Level settings. Returns always a valid path.
		/// If no path has been set it will return a path in the default location using the Assembly-name (%AppData%\Assemblyname)
		/// </summary>
		public string UserConfigPath
		{
			get 
			{
				if (string.IsNullOrEmpty(userconfigpath))
					return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;

				try
				{
					Path.GetFullPath(userconfigpath); //invalid paths will cause an exception
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
		/// <summary>
		/// Gets or sets the path to the Application-Level settings. Returns always a valid path.
		/// If no path has been set it will return a path in the default location using the Assembly-name (%CommonAppData%\Assemblyname)
		/// </summary>
		public string AppConfigPath
		{
			get
			{
				if (string.IsNullOrEmpty(appconfigpath))
					return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Assembly.GetExecutingAssembly().GetName(true).Name;

				try
				{
					Path.GetFullPath(appconfigpath);
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


		private SerializableStringDictionary<object> userconfig = new SerializableStringDictionary<object>();
		private SerializableStringDictionary<object> appconfig = new SerializableStringDictionary<object>();
		private SerializableStringDictionary<object> instconfig = new SerializableStringDictionary<object>();
		/// <summary>
		/// Clones the config object. Creates a new instance and copies all references held in the dictionaries
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
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

		/// <summary>
		/// Event that gets fired when configurations has been changed. If multiple settings has been changed 
		/// ConfigChangedEventArgs.Empty will be used instead of firing event for every single setting
		/// </summary>
		public event EventHandler<ConfigChangedEventArgs> ConfigChanged;

		/// <summary>
		/// Gets or sets a value that indicates if ConfigChanged should fire or not.
		/// This is mainly used in Merge() or around FillDefaults() to prevent spamming events
		/// </summary>
		protected bool SuppressConfigChanged { get; set; }

		/// <summary>
		/// Creates a new, empty config object
		/// </summary>
		public ConfigBase()
		{
			SuppressConfigChanged = true;
			FillDefaults();
			SuppressConfigChanged = false;
		}

		/// <summary>
		/// (Re)Loads the settings in the User-Level config file. Overwrites all entries which are also found in the file
		/// </summary>
		public void LoadUserConfig()
		{
			SuppressConfigChanged = true;
			try
			{
				using (FileStream fs = new FileStream(UserConfigFile, FileMode.Open))
				{
					XmlSerializer x = new XmlSerializer(typeof(SerializableStringDictionary<object>));
					var d = (SerializableStringDictionary<object>)x.Deserialize(fs);
					foreach (var k in d.Keys)
					{
						userconfig[k] = d[k];
					}
				}
			}
			catch
			{

			}
			SuppressConfigChanged = false;
			if (ConfigChanged != null)
				ConfigChanged(this, ConfigChangedEventArgs.Empty);
		}


		/// <summary>
		/// (Re)Loads the settings in the Application-Level config file. Overwrites all entries which are also found in the file
		/// </summary>
		public void LoadAppConfig()
		{
			SuppressConfigChanged = true;
			try
			{
				using (FileStream fs = new FileStream(AppConfigFile, FileMode.Open))
				{
					XmlSerializer x = new XmlSerializer(typeof(SerializableStringDictionary<object>));
					var d = (SerializableStringDictionary<object>)x.Deserialize(fs);
					foreach (var k in d.Keys)
					{
						appconfig[k] = d[k];
					}
				}
			}
			catch
			{

			}
			SuppressConfigChanged = false;
			if (ConfigChanged != null)
				ConfigChanged(this, ConfigChangedEventArgs.Empty);
		}

		/// <summary>
		/// (Re)Loads all settings from the files
		/// </summary>
		public void LoadAll()
		{
			LoadAppConfig();
			LoadUserConfig();
		}

		/// <summary>
		/// Saves the User-Level config into the file
		/// </summary>
		public void SaveUserConfig()
		{
			if (!Directory.Exists(UserConfigPath))
				Directory.CreateDirectory(UserConfigPath);

			using (FileStream fs = new FileStream(UserConfigFile, FileMode.Create))
			{
				XmlSerializer x = new XmlSerializer(typeof(SerializableStringDictionary<object>));
				x.Serialize(fs,userconfig);
			}
		}

		/// <summary>
		/// Saves the Application-Level config to the file
		/// </summary>
		public void SaveAppConfig()
		{
			if (!Directory.Exists(AppConfigPath))
				Directory.CreateDirectory(AppConfigPath);

			using (FileStream fs = new FileStream(AppConfigFile, FileMode.Create))
			{
				XmlSerializer x = new XmlSerializer(typeof(SerializableStringDictionary<object>));
				x.Serialize(fs, appconfig);
			}
		}

		/// <summary>
		/// Saves all settings to the files
		/// </summary>
		public void SaveAll()
		{
			SaveAppConfig();
			SaveUserConfig();
		}

		/// <summary>
		/// Sets a setting
		/// </summary>
		/// <param name="key">The name (ID) of the setting</param>
		/// <param name="o">The object to save</param>
		/// <param name="level">The configuration level</param>
		protected void Set(string key, object o, ConfigLevel level = ConfigLevel.User)
		{
			object old;
			switch(level)
			{
				case ConfigLevel.Application:
					old = appconfig.ContainsKey(key) ? appconfig[key] : null;
					appconfig[key] = o;
					if(!SuppressConfigChanged && ConfigChanged!=null)
						ConfigChanged(this, new ConfigChangedEventArgs(key, level, o, old));					
					break;

				case ConfigLevel.User:
					old = userconfig.ContainsKey(key) ? userconfig[key] : null;
					userconfig[key] = o;
					if(!SuppressConfigChanged && ConfigChanged!=null)
						ConfigChanged(this, new ConfigChangedEventArgs(key, level, o, old));					
					break;

				case ConfigLevel.Instance:
					old = instconfig.ContainsKey(key) ? instconfig[key] : null;
					instconfig[key] = o;
					if (!SuppressConfigChanged && ConfigChanged != null)
						ConfigChanged(this, new ConfigChangedEventArgs(key, level, o, old));	
					break;
			}
		}
		/// <summary>
		/// Gets a setting
		/// </summary>
		/// <typeparam name="T">Type of the read object (the object will be casted to this type)</typeparam>
		/// <param name="key">The name (ID) of the setting</param>
		/// <param name="level">The configuration level</param>
		/// <returns></returns>
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

		/// <summary>
		/// Merges the data of another config file into this instance.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="strat"></param>
		public void Merge(ConfigBase config, MergeStrategy strat = MergeStrategy.Overwrite)
		{
			SuppressConfigChanged = true;
			switch(strat)
			{
				case MergeStrategy.Overwrite:
					foreach (var s in config.userconfig.Keys)
						Set(s, config.userconfig[s], ConfigLevel.User);
					foreach (var s in config.appconfig.Keys)
						Set(s, config.appconfig[s], ConfigLevel.Application);
					foreach (var s in config.instconfig.Keys)
						Set(s, config.instconfig[s], ConfigLevel.Instance);
					break;

				case MergeStrategy.AddNonExistingOnly:
					foreach (var s in config.userconfig.Keys)
						if(!userconfig.ContainsKey(s))
							Set(s, config.userconfig[s], ConfigLevel.User);
					foreach (var s in config.appconfig.Keys)
						if (!appconfig.ContainsKey(s))
							Set(s, config.appconfig[s], ConfigLevel.Application);
					foreach (var s in config.instconfig.Keys)
						if (!instconfig.ContainsKey(s))
							Set(s, config.instconfig[s], ConfigLevel.Instance);
					break;

				case MergeStrategy.UpdateExistingOnly:
					foreach (var s in config.userconfig.Keys)
						if(userconfig.ContainsKey(s))
							Set(s, config.userconfig[s], ConfigLevel.User);
					foreach (var s in config.appconfig.Keys)
						if (appconfig.ContainsKey(s))
							Set(s, config.appconfig[s], ConfigLevel.Application);
					foreach (var s in config.instconfig.Keys)
						if (instconfig.ContainsKey(s))
							Set(s, config.instconfig[s], ConfigLevel.Instance);
					break;
			}
			SuppressConfigChanged = false;
			if (ConfigChanged != null)
				ConfigChanged(this, ConfigChangedEventArgs.Empty);
		}

		/// <summary>
		/// Sets the default-values for all settings
		/// </summary>
		public abstract void FillDefaults();
	}
}
