using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test
{
    public class SubBase2 : Base
    {
        public int Value { get; set; }

        public SubBase2()
        {
            Value = 42;
        }
        public override object Clone()
        {
            return base.Clone<SubBase2>();
        }
    }
    public class SubBase : Base
    {
        public int ValueType { get; set; }

        public SubBase2 ComplexType { get; set; }

        public string ImmutableType { get; set; }

        public SubBase()
        {
            ValueType = 1;
            ComplexType = new SubBase2();
            ImmutableType = "Test";
        }

        public override object Clone()
        {
            return base.Clone<SubBase>();
        }
    }
}
