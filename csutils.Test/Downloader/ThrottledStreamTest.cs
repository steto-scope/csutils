using csutils.Data;
using csutils.Downloader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Downloader
{
	[TestFixture]
	public class ThrottledStreamTest
	{
		[TestCase]
		public void TestStreamRead()
		{
			using (Stream src = new ThrottledStream(new MemoryStream(DummyData.GenerateBytes(1024 * 1024)), 1024*256))
			{
				src.Seek(0, SeekOrigin.Begin);
				byte[] buf = new byte[1024 * 50];
				int read = 1;
				int start = Environment.TickCount;

				while(read>0)
				{
					read = src.Read(buf, 0, buf.Length);						
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 3000);
				Assert.LessOrEqual(elapsed,4000);
			}
			
		}

		[TestCase]
		public void TestStreamWrite()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(), 1024 * 256))
			{
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf = DummyData.GenerateBytes(1024 * 64);
				int read = 1;
				int start = Environment.TickCount;

				for (int i = 0; i < 16; i++)
				{
					tar.Write(buf, 0, buf.Length);
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 3000);
				Assert.LessOrEqual(elapsed, 4000);
			}

		}
	}
}
