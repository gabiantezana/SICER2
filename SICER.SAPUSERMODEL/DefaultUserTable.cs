using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL
{
    public class DefaultUserTable
    {
        [SAPField(IsSystemField = true)]
        public string Code { get; set; }
        [SAPField(IsSystemField = true)]
        public string Name { get; set; }
    }
}
