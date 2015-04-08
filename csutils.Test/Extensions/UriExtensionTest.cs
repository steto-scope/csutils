using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Extensions
{
	[TestFixture]
	public class UriExtensionTest
	{
		[TestCase]
		public void TestIsAbsolute()
		{
			Assert.IsTrue(new Uri("http://google.com").IsAbsolute());
			Assert.IsTrue(new Uri("http://google.com/ressource").IsAbsolute());
			Assert.IsTrue(new Uri("http://google.com/ressource.html").IsAbsolute());
			Assert.IsTrue(new Uri("http://google.com/ressource.html?p=v&a=b#c").IsAbsolute());

			Assert.IsFalse(new Uri("google.com/ressource.html?p=v&a=b#c",UriKind.RelativeOrAbsolute).IsAbsolute());

		}

		[TestCase]
		public void TestIsRelative()
		{
			Assert.IsFalse(new Uri("http://google.com").IsRelative());
			Assert.IsFalse(new Uri("http://google.com/ressource").IsRelative());
			Assert.IsFalse(new Uri("http://google.com/ressource.html").IsRelative());
			Assert.IsFalse(new Uri("http://google.com/ressource.html?p=v&a=b#c").IsRelative());

			Assert.IsTrue(new Uri("google.com/ressource.html?p=v&a=b#c", UriKind.RelativeOrAbsolute).IsRelative());
		}
	}
}
