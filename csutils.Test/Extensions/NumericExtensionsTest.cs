using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Extensions
{
	[TestFixture]
	public class NumericExtensionsTest
	{
		[TestCase]
		public void TestSaturate()
		{
			
		}


		[TestCase]
		public void TestNearestInt()
		{
			Assert.AreEqual(42, (42).Nearest(42));
			Assert.AreEqual(42, (37).Nearest(42));
			Assert.AreEqual(42, (37).Nearest(30,42));
			Assert.AreEqual(32, (37).Nearest(32, 42));
			Assert.AreEqual(1, (0).Nearest(9, 8, 7, 6, 5, 4, 3, 2, 1));
			Assert.AreEqual(1, (0).Nearest(new int[]{9, 8, 7, 6, 5, 4, 3, 2, 1}));

			Assert.AreEqual(0, (0).Nearest(0, 0));
			
			Assert.AreEqual(-42, (-42).Nearest(-42));
			Assert.AreEqual(-42, (-37).Nearest(-42));
			Assert.AreEqual(-42, (-37).Nearest(-30, -42));
			Assert.AreEqual(-32, (-37).Nearest(-32, -42));
			Assert.AreEqual(-1, (0).Nearest(-9, -8, -7, -6, -5, -4, -3, -2, -1));
			Assert.AreEqual(-1, (0).Nearest(new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42, (42).Nearest(-42));
			Assert.AreEqual(42, (42).Nearest(-42, 42));
			Assert.AreEqual(32, (37).Nearest(-32, 32, -42, 42));
			Assert.AreEqual(42, (37).Nearest(-32, 42));
			Assert.AreEqual(42, (0).Nearest(-111,42,77,-42));
		}

		[TestCase]
		public void TestNearestLastInt()
		{
			Assert.AreEqual(42, (42).NearestLast(42));
			Assert.AreEqual(42, (37).NearestLast(42));
			Assert.AreEqual(42, (37).NearestLast(30, 42));
			Assert.AreEqual(42, (37).NearestLast(32, 42));
			Assert.AreEqual(1, (0).NearestLast(9, 8, 7, 6, 5, 4, 3, 2, 1));
			Assert.AreEqual(1, (0).NearestLast(new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0, (0).NearestLast(0, 0));

			Assert.AreEqual(-42, (-42).NearestLast(-42));
			Assert.AreEqual(-42, (-37).NearestLast(-42));
			Assert.AreEqual(-42, (-37).NearestLast(-30, -42));
			Assert.AreEqual(-42, (-37).NearestLast(-32, -42));
			Assert.AreEqual(-1, (0).NearestLast(-9, -8, -7, -6, -5, -4, -3, -2, -1));
			Assert.AreEqual(-1, (0).NearestLast(new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42, (42).NearestLast(-42));
			Assert.AreEqual(42, (42).NearestLast(-42, 42));
			Assert.AreEqual(42, (37).NearestLast(-32, 32, -42, 42));
			Assert.AreEqual(42, (37).NearestLast(-32, 42));
			Assert.AreEqual(-42, (0).NearestLast(-111, 42, 77, -42));
		}



		[TestCase]
		public void TestNearestDouble()
		{
			Assert.AreEqual(42D, (42D).Nearest(42D));
			Assert.AreEqual(42D, (37D).Nearest(42D));
			Assert.AreEqual(42D, (37D).Nearest(30D, 42D));
			Assert.AreEqual(32D, (37D).Nearest(32D, 42D));
			Assert.AreEqual(32D, (37D).Nearest(32, 42F));
			Assert.AreEqual(1D, (0D).Nearest(9D, 8D, 7D, 6D, 5D, 4D, 3D, 2D, 1D));
			Assert.AreEqual(1D, (0D).Nearest(new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0D, (0D).Nearest(0D, 0D));

			Assert.AreEqual(-42D, (-42D).Nearest(-42D));
			Assert.AreEqual(-42D, (-37D).Nearest(-42D));
			Assert.AreEqual(-42D, (-37D).Nearest(-30D, -42D));
			Assert.AreEqual(-32D, (-37D).Nearest(-32D, -42D));
			Assert.AreEqual(-1D, (0D).Nearest(-9D, -8D, -7D, -6D, -5D, -4D, -3D, -2D, -1D));
			Assert.AreEqual(-1D, (0D).Nearest(new double[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42D, (42D).Nearest(-42D));
			Assert.AreEqual(42D, (42D).Nearest(-42D, 42D));
			Assert.AreEqual(32D, (37D).Nearest(-32D, 32D, -42D, 42D));
			Assert.AreEqual(42D, (37D).Nearest(-32D, 42D));
			Assert.AreEqual(42D, (0D).Nearest(-111D, 42D, 77D, -42D));
		}


		[TestCase]
		public void TestNearestLastDouble()
		{
			Assert.AreEqual(42D, (42D).NearestLast(42D));
			Assert.AreEqual(42D, (37D).NearestLast(42D));
			Assert.AreEqual(42D, (37D).NearestLast(30D, 42D));
			Assert.AreEqual(42D, (37D).NearestLast(32D, 42D));
			Assert.AreEqual(42D, (37D).NearestLast(32, 42F));
			Assert.AreEqual(1D, (0D).NearestLast(9D, 8D, 7D, 6D, 5D, 4D, 3D, 2D, 1D));
			Assert.AreEqual(1D, (0D).NearestLast(new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0D, (0D).NearestLast(0D, 0D));

			Assert.AreEqual(-42D, (-42D).NearestLast(-42D));
			Assert.AreEqual(-42D, (-37D).NearestLast(-42D));
			Assert.AreEqual(-42D, (-37D).NearestLast(-30D, -42D));
			Assert.AreEqual(-42D, (-37D).NearestLast(-32D, -42D));
			Assert.AreEqual(-1D, (0D).NearestLast(-9D, -8D, -7D, -6D, -5D, -4D, -3D, -2D, -1D));
			Assert.AreEqual(-1D, (0D).NearestLast(new double[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42D, (42D).NearestLast(-42D));
			Assert.AreEqual(42D, (42D).NearestLast(-42D, 42D));
			Assert.AreEqual(42D, (37D).NearestLast(-32D, 32D, -42D, 42D));
			Assert.AreEqual(42D, (37D).NearestLast(-32D, 42D));
			Assert.AreEqual(-42D, (0D).NearestLast(-111D, 42D, 77D, -42D));
		}



		[TestCase]
		public void TestFarestInt()
		{
			Assert.AreEqual(42, (42).Farest(42));
			Assert.AreEqual(42, (37).Farest(42));
			Assert.AreEqual(30, (37).Farest(30, 42));
			Assert.AreEqual(32, (37).Farest(32, 42));
			Assert.AreEqual(9, (0).Farest(9, 8, 7, 6, 5, 4, 3, 2, 1));
			Assert.AreEqual(9, (0).Farest(new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0, (0).Farest(0, 0));

			Assert.AreEqual(-42, (-42).Farest(-42));
			Assert.AreEqual(-42, (-37).Farest(-42));
			Assert.AreEqual(-30, (-37).Farest(-30, -42));
			Assert.AreEqual(-32, (-37).Farest(-32, -42));
			Assert.AreEqual(-9, (0).Farest(-9, -8, -7, -6, -5, -4, -3, -2, -1));
			Assert.AreEqual(-9, (0).Farest(new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42, (42).Farest(-42));
			Assert.AreEqual(-42, (42).Farest(-42, 42));
			Assert.AreEqual(-42, (37).Farest(-32, 32, -42, 42));
			Assert.AreEqual(-32, (37).Farest(-32, 42));
			Assert.AreEqual(-111, (0).Farest(-111, 42, 77, -42));
		}

		[TestCase]
		public void TestFarestLastInt()
		{
			Assert.AreEqual(42, (42).FarestLast(42));
			Assert.AreEqual(42, (37).FarestLast(42));
			Assert.AreEqual(30, (37).FarestLast(30, 42));
			Assert.AreEqual(42, (37).FarestLast(32, 42));
			Assert.AreEqual(9, (0).FarestLast(9, 8, 7, 6, 5, 4, 3, 2, 1));
			Assert.AreEqual(9, (0).FarestLast(new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0, (0).FarestLast(0, 0));

			Assert.AreEqual(-42, (-42).FarestLast(-42));
			Assert.AreEqual(-42, (-37).FarestLast(-42));
			Assert.AreEqual(-30, (-37).FarestLast(-30, -42));
			Assert.AreEqual(-42, (-37).FarestLast(-32, -42));
			Assert.AreEqual(-9, (0).FarestLast(-9, -8, -7, -6, -5, -4, -3, -2, -1));
			Assert.AreEqual(-9, (0).FarestLast(new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42, (42).FarestLast(-42));
			Assert.AreEqual(-42, (42).FarestLast(-42, 42));
			Assert.AreEqual(-42, (37).FarestLast(-32, 32, -42, 42));
			Assert.AreEqual(-32, (37).FarestLast(-32, 42));
			Assert.AreEqual(-111, (0).FarestLast(-111, 42, 77, -42));
		}

		[TestCase]
		public void TestFarestDouble()
		{
			Assert.AreEqual(42D, (42D).Farest(42D));
			Assert.AreEqual(42D, (37D).Farest(42D));
			Assert.AreEqual(30D, (37D).Farest(30D, 42D));
			Assert.AreEqual(32D, (37D).Farest(32D, 42D));
			Assert.AreEqual(32D, (37D).Farest(32, 42F));
			Assert.AreEqual(9D, (0D).Farest(9D, 8D, 7D, 6D, 5D, 4D, 3D, 2D, 1D));
			Assert.AreEqual(9D, (0D).Farest(new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0D, (0D).Farest(0D, 0D));

			Assert.AreEqual(-42D, (-42D).Farest(-42D));
			Assert.AreEqual(-42D, (-37D).Farest(-42D));
			Assert.AreEqual(-30D, (-37D).Farest(-30D, -42D));
			Assert.AreEqual(-32D, (-37D).Farest(-32D, -42D));
			Assert.AreEqual(-9D, (0D).Farest(-9D, -8D, -7D, -6D, -5D, -4D, -3D, -2D, -1D));
			Assert.AreEqual(-9D, (0D).Farest(new double[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42D, (42D).Farest(-42D));
			Assert.AreEqual(-42D, (42D).Farest(-42D, 42D));
			Assert.AreEqual(-42D, (37D).Farest(-32D, 32D, -42D, 42D));
			Assert.AreEqual(-32D, (37D).Farest(-32D, 42D));
			Assert.AreEqual(-111D, (0D).Farest(-111D, 42D, 77D, -42D));
		}

		[TestCase]
		public void TestFarestLastDouble()
		{
			Assert.AreEqual(42D, (42D).FarestLast(42D));
			Assert.AreEqual(42D, (37D).FarestLast(42D));
			Assert.AreEqual(30D, (37D).FarestLast(30D, 42D));
			Assert.AreEqual(42D, (37D).FarestLast(32D, 42D));
			Assert.AreEqual(42D, (37D).FarestLast(32, 42F));
			Assert.AreEqual(9D, (0D).FarestLast(9D, 8D, 7D, 6D, 5D, 4D, 3D, 2D, 1D));
			Assert.AreEqual(9D, (0D).FarestLast(new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));

			Assert.AreEqual(0D, (0D).FarestLast(0D, 0D));

			Assert.AreEqual(-42D, (-42D).FarestLast(-42D));
			Assert.AreEqual(-42D, (-37D).FarestLast(-42D));
			Assert.AreEqual(-30D, (-37D).FarestLast(-30D, -42D));
			Assert.AreEqual(-42D, (-37D).FarestLast(-32D, -42D));
			Assert.AreEqual(-9D, (0D).FarestLast(-9D, -8D, -7D, -6D, -5D, -4D, -3D, -2D, -1D));
			Assert.AreEqual(-9D, (0D).FarestLast(new double[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 }));

			Assert.AreEqual(-42D, (42D).FarestLast(-42D));
			Assert.AreEqual(-42D, (42D).FarestLast(-42D, 42D));
			Assert.AreEqual(-42D, (37D).FarestLast(-32D, 32D, -42D, 42D));
			Assert.AreEqual(-32D, (37D).FarestLast(-32D, 42D));
			Assert.AreEqual(-111D, (0D).FarestLast(-111D, 42D, 77D, -42D));
		}
	}
}
