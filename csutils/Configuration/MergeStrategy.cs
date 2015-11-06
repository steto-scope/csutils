using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Configuration
{
	/// <summary>
	/// Represents the Strategy of the Configuration Merge
	/// </summary>
	public enum MergeStrategy
	{
		/// <summary>
		/// Simply takes the new value and stores it into the receiving object. 
		/// </summary>
		Overwrite,
		/// <summary>
		/// Overwrites a value only if the config entry already exists (does not add new ones)
		/// </summary>
		UpdateExistingOnly,
		/// <summary>
		/// New values will be added only if the key doesn´t exist. Does not overwrite any values. 
		/// </summary>
		AddNonExistingOnly
	}
}
