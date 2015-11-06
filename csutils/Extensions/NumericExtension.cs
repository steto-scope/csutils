using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extensions for primitive numeric types
    /// </summary>
    public static class csutilsNumericExtension
    {      
		/// <summary>
		/// Determines the nearest value to the integer. If multiple numbers have the same distance the first found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static int Nearest(this int number, params int[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			int val = 0;
			int dist = int.MaxValue;

			for (int i = 0; i < values.Length; i++)
			{
				if( Math.Abs(values[i] - number) < dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}


		/// <summary>
		/// Determines the nearest value to the integer. If multiple numbers have the same distance the last found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static int NearestLast(this int number, params int[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			int val = 0;
			int dist = int.MaxValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) <= dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

		/// <summary>
		/// Determines the farest value to the integer. If multiple numbers have the same distance the first found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static int Farest(this int number, params int[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			int val = 0;
			int dist = int.MinValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) > dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

		/// <summary>
		/// Determines the farest value to the integer. If multiple numbers have the same distance the last found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static int FarestLast(this int number, params int[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			int val = 0;
			int dist = int.MinValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) >= dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}



		/// <summary>
		/// Determines the nearest value to the double. If multiple numbers have the same distance the first found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static double Nearest(this double number, params double[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			double val = 0;
			double dist = double.MaxValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) < dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

		/// <summary>
		/// Determines the nearest value to the double. If multiple numbers have the same distance the last found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static double NearestLast(this double number, params double[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			double val = 0;
			double dist = double.MaxValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) <= dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

		/// <summary>
		/// Determines the farest value to the double. If multiple numbers have the same distance the first found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static double Farest(this double number, params double[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			double val = 0;
			double dist = double.MinValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) > dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

		/// <summary>
		/// Determines the farest value to the double. If multiple numbers have the same distance the last found will be returned
		/// </summary>
		/// <param name="number"></param>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException">values can not be null</exception>
		/// <returns></returns>
		public static double FarestLast(this double number, params double[] values)
		{
			if (values == null)
				throw new ArgumentException("values can not be null");

			double val = 0;
			double dist = double.MinValue;

			for (int i = 0; i < values.Length; i++)
			{
				if (Math.Abs(values[i] - number) >= dist)
				{
					dist = Math.Abs(values[i] - number);
					val = values[i];
				}
			}
			return val;
		}

        /// <summary>
        /// Converts a size to a filesize string
        /// </summary>
        /// <param name="size">original size</param>
        /// <param name="target">target unit</param>
        /// <param name="given">original unit, Byte is default</param>
        /// <returns></returns>
        public static string FormatFilesize(this long size, FileSizeUnit target = FileSizeUnit.Auto, FileSizeUnit given = FileSizeUnit.Byte, string stringFormat = "{0:0.##}")
        {
            string[] unit = new string[]{"","B","KB","MB","GB","TB","PB","EB"};
            
            given = given == FileSizeUnit.Auto ? (FileSizeUnit)1 : given; //Auto == Byte for given bytes
            long sizeinbyte = (long)(size*Math.Pow(1024,(int)given-1)); //size in byte
            int steps = (int)Math.Floor(Math.Log(sizeinbyte, 1024)); //number of 1024er steps inside s (lower bound)
           
            int upper = (int)target;
            if(upper == int.MaxValue)
                upper = steps+1;

            double res = sizeinbyte * Math.Pow(1024, -(upper-1));
            return string.Format(stringFormat+" {1}", res, unit[upper]);
        }
    }

    /// <summary>
    /// Unit of Filesizes
    /// </summary>
    public enum FileSizeUnit : int
    {
        /// <summary>
        /// Unit is chosen automatically
        /// </summary>
        Auto = int.MaxValue,
        /// <summary>
        /// Byte
        /// </summary>
        Byte = 1,
        /// <summary>
        /// Kilobyte
        /// </summary>
        Kilobyte = 2,
        /// <summary>
        /// Megabyte
        /// </summary>
        Megabyte = 3,
        /// <summary>
        /// Gigabyte
        /// </summary>
        Gigabyte = 4,
        /// <summary>
        /// Terabyte
        /// </summary>
        Terabyte = 5,
        /// <summary>
        /// Petabyte
        /// </summary>
        Petabyte = 6,
        /// <summary>
        /// Exabyte
        /// </summary>
        Exabyte = 7
    }
}
