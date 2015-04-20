using csutils.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Configuration
{
	[TestFixture]
	class ConfigBaseTest
	{
		[TestCase]
		public void TestStorage()
		{
			SampleConfig sc = new SampleConfig();
			sc.SaveAll();

			SampleConfig sc2 = new SampleConfig();
			sc2.LoadAll();
		}

		[TestCase]
		public void TestChanges()
		{
			SampleConfig sc = new SampleConfig();
			sc.Test = 100;
			sc.Test2 = 700;
			Assert.AreEqual(0, sc.Test3);
			sc.Test3 = 666;
			Assert.AreEqual(666, sc.Test3);
			sc.SaveAll();

			SampleConfig sc2 = new SampleConfig();
			Assert.AreEqual(0, sc2.Test3);
			Assert.AreEqual(42, sc2.Test);
			Assert.AreEqual(13, sc2.Test2);
			Assert.AreEqual("Foo", sc2.TC.Something);
			sc2.LoadAll();
			Assert.AreEqual(0, sc2.Test3);
			Assert.AreEqual(100, sc2.Test);
			Assert.AreEqual(700, sc2.Test2);
			Assert.AreEqual("Foo", sc2.TC.Something);

		}

		[TestCase]
		public void TestMerge()
		{
			SampleConfig sc; SampleConfig sc2;
			
			sc = new SampleConfig();
			sc.Test = 100;
			sc.Test2 = 700;
			sc.Test3 = 666;

			sc2 = new SampleConfig();
			sc2.Test = 77;
			sc2.Test2 = 700;

			sc.Merge(sc2,MergeStrategy.Overwrite);
			Assert.AreEqual(77, sc.Test);
			Assert.AreEqual(700, sc.Test2);
			Assert.AreEqual(666, sc.Test3);




			sc = new SampleConfig();
			sc.Test = 100;
			sc.Test2 = 700;
			sc.Test3 = 666;

			sc2 = new SampleConfig();
			sc2.Test = 77;
			sc2.Test2 = 700;

			sc.Merge(sc2, MergeStrategy.AddNonExistingOnly);
			Assert.AreEqual(100, sc.Test);
			Assert.AreEqual(700, sc.Test2);
			Assert.AreEqual(666, sc.Test3);




			sc = new SampleConfig();
			sc.Test = 100;
			sc.Test2 = 700;			

			sc2 = new SampleConfig();
			sc2.Test = 77;
			sc2.Test2 = 800;
			sc2.Test3 = 666;

			sc.Merge(sc2, MergeStrategy.UpdateExistingOnly);
			Assert.AreEqual(77, sc.Test);
			Assert.AreEqual(800, sc.Test2);
			Assert.AreEqual(0, sc.Test3);

		}
	}
}
