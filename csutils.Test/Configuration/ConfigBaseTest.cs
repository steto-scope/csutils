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
			sc.Save("sample.xml");

			SampleConfig sc2 = new SampleConfig();
			sc2.Load("sample.xml");
		}

		[TestCase]
		public void TestSaveUser()
		{

		}
	}
}
