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
			get { return (int)(bandwidthlimit*10); }
			set 
			{
				if (value < 1)
					throw new ArgumentException("BandwidthLimit has to be >0");

				bandwidthlimit = value/10; 
			}
		}

		



		#endregion


		#region Private Members

		private int processed;
		System.Timers.Timer resettimer;
		AutoResetEvent wh = new AutoResetEvent(true);
		private Stream parent;
		#endregion

		/// <summary>
		/// Creates a new Stream with Databandwith cap
		/// </summary>
		/// <param name="parentStream"></param>
		/// <param name="maxBytesPerSecond"></param>
		public ThrottledStream(Stream parentStream, int maxBytesPerSecond=int.MaxValue) 
		{

			BandwidthLimit = maxBytesPerSecond;
			parent = parentStream;
			processed = 0;
			resettimer = new System.Timers.Timer();
			resettimer.Interval = 100;
			resettimer.Elapsed += resettimer_Elapsed;
			resettimer.Start();			
		}


		private void resettimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			processed = 0;
			wh.Set();
		}

		#region Stream-Overrides

		/// <inheritdoc />
		public override void Close()
		{
			resettimer.Stop();
			resettimer.Close();
			base.Close();
		}
		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			resettimer.Dispose();
			base.Dispose(disposing);
		}
		/// <inheritdoc />
		public override bool CanRead
		{
			get { return parent.CanRead; }
		}
		/// <inheritdoc />
		public override bool CanSeek
		{
			get { return parent.CanSeek; }
		}
		/// <inheritdoc />
		public override bool CanWrite
		{
			get { return parent.CanWrite; }
		}
		/// <inheritdoc />
		public override void Flush()
		{
			parent.Flush();
		}
		/// <inheritdoc />
		public override long Length
		{
			get { return parent.Length; }
		}
		/// <inheritdoc />
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
		/// <inheritdoc />
		public override int Read(byte[] buffer, int offset, int count)
		{
			int read = 0;
			//case 1: everything fits into this cycle
			//case 2: nothing fits into this cycle, but 1 cycle would be enough 
			//case 3: everything would fit into 1 cycle, but the current cycle has not enough space so 2 cycles overlap
			//case 4: many cycles are needed (processed ignored in the first, would cause more problems than use)


			//case 1
			if(processed+count < bandwidthlimit)
			{
				processed += count;
				return parent.Read(buffer, offset, count);
			}

			//case 2
			if(processed == bandwidthlimit && count < bandwidthlimit)
			{
				wh.WaitOne();
				processed += count;
				return parent.Read(buffer, offset, count);
			}

			//case 3
			if(count < bandwidthlimit && processed+count > bandwidthlimit)
			{
				int first = bandwidthlimit - processed;
				int second = count - first;
				read = parent.Read(buffer, offset, first);
				wh.WaitOne();
				read += parent.Read(buffer, offset + read, second);
				processed += second;
				return read;				
			}
			
			//case 4
			if(count > bandwidthlimit)
			{
				int current = 0;				
				for(int i=0; i<count; i+=current)
				{
					current = Math.Min(count-i, bandwidthlimit);
					read += parent.Read(buffer, offset + i, current);
					if (current == bandwidthlimit)
						wh.WaitOne();
				}
				processed += current;
				return read;
			}

			return 0;
		}
		/// <inheritdoc />
		public override long Seek(long offset, SeekOrigin origin)
		{
			return parent.Seek(offset, origin);
		}
		/// <inheritdoc />
		public override void SetLength(long value)
		{
			parent.SetLength(value);
		}
		/// <inheritdoc />
		public override void Write(byte[] buffer, int offset, int count)
		{
			int current = 0;
			for (int i = 0; i < count; i+=current )
			{
				current = Math.Min(count-i, bandwidthlimit);
				parent.Write(buffer, offset+i, current);
				if (current == bandwidthlimit)
					wh.WaitOne();
			}			
			
		}

		#endregion

		
	}
}
