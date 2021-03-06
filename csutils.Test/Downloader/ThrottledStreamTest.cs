﻿using csutils.Data;
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
			using (Stream src = new ThrottledStream(new MemoryStream(DummyData.GenerateRandomBytes(1024)),256))
			{
				src.Seek(0, SeekOrigin.Begin);
				byte[] buf = new byte[256];
				int read = 1;
				int start = Environment.TickCount;

				while(read>0)
				{
					read = src.Read(buf, 0, buf.Length);						
				}

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 4000);
			}
			
		}

		[TestCase]
		public void TestStreamWrite()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(),256))
			{
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf = DummyData.GenerateRandomBytes(1024);
				int start = Environment.TickCount;

				tar.Write(buf, 0, buf.Length);
				

				int elapsed = Environment.TickCount - start;
				Assert.GreaterOrEqual(elapsed, 4000);
			}
		}


		[TestCase]
		public void TestStreamIntegrity()
		{
			using (Stream tar = new ThrottledStream(new MemoryStream(), 100))
			{
				byte[] buf = DummyData.GenerateOrderedBytes(500);
				tar.Write(buf, 0, buf.Length);
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf2 = new byte[500];
				tar.Read(buf2, 0, buf2.Length);
				Assert.IsTrue(buf.SequenceEqual(buf2));
			}

			using (Stream tar = new ThrottledStream(new MemoryStream()))
			{
				byte[] buf = DummyData.GenerateOrderedBytes(4096);
				tar.Write(buf, 0, buf.Length);
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf2 = new byte[4096];
				tar.Read(buf2, 0, buf2.Length);
				Assert.IsTrue(buf.SequenceEqual(buf2));
			}

			using (Stream tar = new ThrottledStream(new MemoryStream(), 77))
			{
				byte[] buf = DummyData.GenerateOrderedBytes(247);
				tar.Write(buf, 0, buf.Length);
				tar.Seek(0, SeekOrigin.Begin);
				byte[] buf2 = new byte[247];
				tar.Read(buf2, 0, buf2.Length);
				Assert.IsTrue(buf.SequenceEqual(buf2));
			}
		}

	}
}
