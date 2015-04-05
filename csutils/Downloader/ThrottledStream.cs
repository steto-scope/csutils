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
	/// <summary>
	/// Stream that limits the maximal bandwith. 
	/// If the internal counter exceeds the MaxBytePerSecond-Value in under 1s the AutoResetEvent blocks the stream until the second finally elapsed
	/// </summary>
	public class ThrottledStream : Stream
	{
		#region Properties

		private int bandwidthlimit;
		/// <summary>
		/// Bandwith Limit (in B/s)
		/// </summary>
		public int BandwidthLimit
		{
			get { return bandwidthlimit; }
			set 
			{
				if (value < 1)
					throw new ArgumentException("BandwidthLimit has to be >0");

				bandwidthlimit = value; 
			}
		}

		



		#endregion


		#region Private Members

		private int processed;
		System.Timers.Timer resettimer;
		AutoResetEvent wh = new AutoResetEvent(true);
		private Stream parent;
		private int cycles;
		#endregion

		/// <summary>
		/// Creates a new Stream with Databandwith cap
		/// </summary>
		/// <param name="parentStream"></param>
		/// <param name="maxBytesPerSecond"></param>
		public ThrottledStream(Stream parentStream, int maxBytesPerSecond=int.MaxValue, int updateCycles=1) 
		{
			if (updateCycles < 1)
				throw new ArgumentException("updateCycles has to be >0");
			cycles = updateCycles;

			BandwidthLimit = maxBytesPerSecond;
			parent = parentStream;
			processed = 0;
			resettimer = new System.Timers.Timer();
			resettimer.Interval = 1000 / updateCycles;
			resettimer.Elapsed += resettimer_Elapsed;
			resettimer.Start();			
		}

		protected void Throttle(int bytes)
		{
			try
			{
				processed += bytes;
				if (processed >= bandwidthlimit/cycles)
					wh.WaitOne();
			}
			catch
			{
			}
		}

		private void resettimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			processed = 0;
			wh.Set();
		}

		#region Stream-Overrides

		public override void Close()
		{
			resettimer.Stop();
			resettimer.Close();
			base.Close();
		}
		protected override void Dispose(bool disposing)
		{
			resettimer.Dispose();
			base.Dispose(disposing);
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
			int step = bandwidthlimit / cycles;
			int len;
			int read = 0;
			for (int i = 0; i < count; i += step)
			{
				len = count - i < step ? count - i : step;
				Throttle(len);
				read += parent.Read(buffer, offset + i, len);
			}
			return read;			
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
			int step = bandwidthlimit/cycles;
			int len;
			for (int i = 0; i < count; i += step)
			{
				len = count-i<step ? count-i: step;
				Throttle(len);
				parent.Write(buffer, offset+i, len);
			}
		}

		#endregion

		
	}
}
