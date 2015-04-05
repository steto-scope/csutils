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
			using (Stream src = new ThrottledStream(new MemoryStream(DummyData.GenerateRandomBytes(1024 * 1024)), 1024*256))
			{
				src.Seek(0, SeekOrigin.Begin);
				byte[] buf = new byte[1024 * 256];
				int read = 1;
				int start = Environment.TickCount;

				while(read>0)
				{
					read = src.Read(buf, 0, buf.Length);						
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 3500);
				Assert.LessOrEqual(elapsed,4500);
			}
			
		}

		[TestCase]
		public void TestStreamWrite()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(), 1024 * 256))
			{
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf = DummyData.GenerateRandomBytes(1024 * 256);
				int start = Environment.TickCount;

				for (int i = 0; i < 4; i++)
				{
					tar.Write(buf, 0, buf.Length);
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 2500);
				Assert.LessOrEqual(elapsed, 3500);
			}
		}


		[TestCase]
		public void TestStreamReadHighCycle()
		{
			using (Stream src = new ThrottledStream(new MemoryStream(DummyData.GenerateRandomBytes(1024 * 1024)), 1024 * 256, 10))
			{
				src.Seek(0, SeekOrigin.Begin);
				byte[] buf = new byte[1024 * 256];
				int read = 1;
				int start = Environment.TickCount;

				while (read > 0)
				{
					read = src.Read(buf, 0, buf.Length);
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 4500);
				Assert.LessOrEqual(elapsed, 5500);
			}

		}

		[TestCase]
		public void TestStreamWriteHighCycle()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(), 1024 * 256,10))
			{
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf = DummyData.GenerateRandomBytes(1024 * 256);
				int start = Environment.TickCount;
						
				//todo: writehighcycle is too fast
				tar.Write(buf, 0, buf.Length);				

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 3500);
				Assert.LessOrEqual(elapsed, 4500);
			}
		}


		[TestCase]
		public void TestStreamIntegrity()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(), 100,6))
			{
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf = DummyData.GenerateOrderedBytes(500);
				
				tar.Write(buf, 0, buf.Length);			

				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf2 = new byte[500];
				int read = tar.Read(buf2, 0, buf2.Length);

				Assert.IsTrue(buf.SequenceEqual(buf2));
			}
		}

	}
}
