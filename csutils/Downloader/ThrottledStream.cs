using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace csutils.Downloader
{
	public class ThrottledStream : Stream
	{
		private int maxBytePerSecond;

		public int MaxBytePerSecond
		{
			get { return maxBytePerSecond; }
			set 
			{
				if (value < 1)
					throw new ArgumentException("MaxBytePerSecond has to be greater than 0");

				maxBytePerSecond = value; 
			}
		}

		private int start;
		private int processed;
		System.Timers.Timer ticktimer;




		private Stream parent;
		public ThrottledStream(Stream parentStream)
		{
			parent = parentStream;
			processed = 0;
			ticktimer = new System.Timers.Timer();
			ticktimer.Interval = 1000;
			ticktimer.Elapsed += ticktimer_Elapsed;
			ticktimer.Start();
		}

		public ThrottledStream(Stream parentStream, int bps) : this(parentStream)
		{
			MaxBytePerSecond = bps;
		}
		
		public override void Close()
		{
			ticktimer.Stop();
			ticktimer.Close();
			base.Close();
		}
		protected override void Dispose(bool disposing)
		{
			
			ticktimer.Dispose();
			base.Dispose(disposing);
		}

		private void ticktimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			start = Environment.TickCount;
			processed = 0;
		}

		public override bool CanRead
		{
			get { return parent.CanRead; }
		}

		public override bool CanSeek
		{
			get { return parent.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return parent.CanWrite; }
		}

		public override void Flush()
		{
			parent.Flush();
		}

		public override long Length
		{
			get { return parent.Length; }
		}

		public override long Position
		{
			get
			{
				return parent.Position;
			}
			set
			{
				parent.Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			Throttle(count);

			return parent.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return parent.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			parent.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Throttle(count);

			parent.Write(buffer, offset, count);
		}

		protected void Throttle(int bytes)
		{
			if (bytes <= 0 || MaxBytePerSecond <= 0)
				return;

			processed += bytes;
			
			if(processed >= maxBytePerSecond)
			{
				try
				{
					Thread.Sleep(Environment.TickCount - start + 1);
				}
				catch
				{

				}
			}
		}
	}
}
