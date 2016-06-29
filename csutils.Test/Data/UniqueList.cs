using csutils.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Data
{
    [TestFixture]
    public class UniqueListTest
    {
        [TestCase]
        public void TestAdd()
        {
            UniqueList<int> l = new UniqueList<int>();
            l.Add(1);
            Assert.AreEqual(1, l.Count);
            l.Add(1);
            Assert.AreEqual(1, l.Count);
            l.Add(2);
            Assert.AreEqual(2, l.Count);

            l.AddRange(new int[] { 2, 3, 4 });
            Assert.AreEqual(4, l.Count);
            l.AddRange(new int[] { 1, 2, 3, 4 });
            Assert.AreEqual(4, l.Count);

            l.Insert(0, 42);
            Assert.AreEqual(42, l[0]);
            Assert.AreEqual(5, l.Count);

            l.InsertRange(0, new int[] { 2, 99 });
            Assert.AreEqual(99, l[0]);
            Assert.AreEqual(6, l.Count);
        }


        [TestCase]
        public void TestRemove()
        {
            UniqueList<int> l = new UniqueList<int>();
            l.AddRange(new int[] { 1,2,3,4,5,6 });
            l.Remove(6);
            Assert.AreEqual(5, l.Count);

            l.RemoveRange(0, 2);
            Assert.AreEqual(3, l.Count);

            l.RemoveAt(0);
            Assert.AreEqual(2, l.Count);
            Assert.AreEqual(4, l[0]);          
            
        }
    }
}
