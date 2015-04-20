using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csutils.Data
{
	public class Pair<A,B>
	{
		public Pair()
		{

		}

		private A a;
		[XmlAttribute("Name")]
		public A Item1
		{
			get { return a; }
			set { a = value; }
		}

		private B b;

		[XmlAttribute("Level")]
		public B Item2
		{
			get { return b; }
			set { b = value; }
		}

		public Pair(A a, B b)
		{
			Item1 = a;
			Item2 = b;
		}

		public override int GetHashCode()
		{
			int ah = Item1!=null ?  Item1.GetHashCode() : 1;
			int bh = Item2!=null ? Item2.GetHashCode() : 1;
			return ah * bh;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is Pair<A, B>))
				return false;

			return Item1.Equals(((Pair<A, B>)obj).Item1) && Item2.Equals(((Pair<A, B>)obj).Item2);
		}
	}
}
