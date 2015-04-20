using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csutils.Configuration
{

	public enum ConfigLevel : int
	{
		[XmlEnum("Instance")]
		Instance,
		[XmlEnum("User")]
		User,
		[XmlEnum("Application")]
		Application		
	}
}
