using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Configuration
{
	public class SampleConfig : ConfigBase
	{
		public int Test { get { return Get<int>("Test"); } set { Set("Test", value); } }
		public int Test2 { get { return Get<int>("Test2",ConfigLevel.Application); } set { Set("Test2", value,ConfigLevel.Application); } }

		public TestClass TC { get { return Get<TestClass>("Test"); } set { Set("Test", value); } }


		public override void FillDefaults()
		{
			Test = 42;
			TC = new TestClass() { Something = "Foo" };
			Test2 = 13;
		}
	}

	public class TestClass
	{
		public string Something { get; set; }
	}
}
