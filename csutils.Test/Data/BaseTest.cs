using System;
using csutils.Data;
using NUnit.Framework;
using csutils.Test;

namespace REC.Test.Data
{
    
    [TestFixture]
    public class BaseTest
    {
        [TestCase]
        public void TestClone()
        {
            SubBase b = new SubBase();
            Assert.AreEqual(1, b.ValueType);
            Assert.AreEqual("Test", b.ImmutableType);
            Assert.IsNotNull(b.ComplexType);
            Assert.AreEqual(42, b.ComplexType.Value);

            b.ComplexType.Value = 999;
            Assert.AreEqual(999, b.ComplexType.Value);

            SubBase b2 = (SubBase)b.Clone();
            b2.ComplexType.Value = 100;
            b2.ImmutableType = "Test2";
            Assert.AreEqual(999, b.ComplexType.Value);
            Assert.AreEqual(100, b2.ComplexType.Value);
            Assert.AreNotEqual(b.ComplexType, b2.ComplexType);
            Assert.AreNotEqual(b, b2);
            Assert.AreNotEqual(b.ImmutableType, b2.ImmutableType);
            

        }
    }
}
