using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Extensions
{
	[TestFixture]
	public class FileInfoExtensionTest
	{
#if CLR
		[TestCase]
		public void TestCanWrite()
		{
			Assert.IsTrue(new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\test.txt").CanWrite());
			Assert.IsFalse(new FileInfo("C:\\Windows\\System32\\sethc.exe").CanWrite());
			try
			{
				new FileInfo("C:\\Windows\\System32?\\sethc.exe").CanWrite();
				Assert.Fail();
			}
			catch { }
		}
#endif
	}
}
