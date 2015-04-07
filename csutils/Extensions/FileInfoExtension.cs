using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary>
	/// FileInfoExtensions
	/// </summary>
	public static class csutilsFileInfoExtension
	{
#if CLR
		/// <summary>
		/// Checks if Write-Permissions for this Location are granted
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool CanWrite(this FileInfo info)
		{
			try
			{
				return info.Directory.CanWrite();
			}
			catch
			{
				return false;
			}
		}
#endif
	}

	/// <summary>
	/// DirectoryInfoExtensions
	/// </summary>
	public static class csutilsDirectoryInfoExtension
	{
#if CLR
		/// <summary>
		/// Checks if Write-Permissions for this Location are granted
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool CanWrite(this DirectoryInfo info)
		{
			try
			{
				string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
				AuthorizationRuleCollection rules = info.GetAccessControl(AccessControlSections.All).GetAccessRules(true, true, typeof(NTAccount));

				foreach (AuthorizationRule rule in rules)
					if (rule.IdentityReference.Value.Equals(username, StringComparison.CurrentCultureIgnoreCase))
						return ((((FileSystemAccessRule)rule).FileSystemRights & FileSystemRights.WriteData) > 0);									
			}
			catch {	}

			return false;
		}
#endif
	}
}
