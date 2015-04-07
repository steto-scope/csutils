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
    }
}
