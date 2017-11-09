using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.HELPER
{
    public class ComparerHelper
    {
        public class OcrdComparer : IEqualityComparer<OCRD>
        {
            public bool Equals(OCRD x, OCRD y)
            {
                return x.CardCode == y.CardCode;
            }

            public int GetHashCode(OCRD obj)
            {
                return 37 * obj.CardCode.GetHashCode() + 19 * obj.CardCode.GetHashCode();
            }
        }
    }

}
