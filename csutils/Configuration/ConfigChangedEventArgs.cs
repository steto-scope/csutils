using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Configuration
{
	/// <summary>
	/// EventArgs for ConfigBase.ConfigChanged
	/// </summary>
	public class ConfigChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Generic ConfigChangedEventArgs-Object. This is used when multiple changes at once occour
		/// </summary>
		public static new readonly ConfigChangedEventArgs Empty = new ConfigChangedEventArgs();
		/// <summary>
		/// Key
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// Level
		/// </summary>
		public ConfigLevel Level { get; set; }
		/// <summary>
		/// New Value
		/// </summary>
		public object NewValue { get; set; }
		/// <summary>
		/// Old Value
		/// </summary>
		public object OldValue { get; set; }
		/// <summary>
		/// Creates a new object
		/// </summary>
		/// <param name="k"></param>
		/// <param name="l"></param>
		/// <param name="newValue"></param>
		/// <param name="oldValue"></param>
		public ConfigChangedEventArgs(string k, ConfigLevel l, object newValue, object oldValue)
		{
			Key = k;
			Level = l;
			NewValue = newValue;
			OldValue = oldValue;
		}

		private ConfigChangedEventArgs()
		{

		}
	}
}
