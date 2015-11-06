using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace csutils.Configuration
{
	/// <summary>
	/// The Level of configs
	/// </summary>
	public enum ConfigLevel : int
	{
		/// <summary>
		/// Instance-Level settings will never be saved, they´re lost after the config-object gets destroyed
		/// </summary>
		[XmlEnum("Instance")]
		Instance,
		/// <summary>
		/// User-Level settings will be saved on user-basis. The config-file is typically found in the AppData-
		/// Directory of the current user
		/// </summary>
		[XmlEnum("User")]
		User,
		/// <summary>
		/// Application-Level settings are meant to use Application-wide, regardless of the current user
		/// </summary>
		[XmlEnum("Application")]
		Application		
	}
}
