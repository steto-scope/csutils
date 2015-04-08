using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	/// <summary>
	/// Uri Extensions
	/// </summary>
	public static class csutilsUriExtension
	{
		/// <summary>
		/// Checks if a URI is absolute. 
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static bool IsAbsolute(this Uri uri)
		{
			Uri u;
			try
			{
				return Uri.TryCreate(uri.ToString(), UriKind.Absolute, out u);
			}
			catch { return false; }
		}

		/// <summary>
		/// Checks if a URI is relative
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static bool IsRelative(this Uri uri)
		{
			return !uri.IsAbsolute();
		}


	}
}
