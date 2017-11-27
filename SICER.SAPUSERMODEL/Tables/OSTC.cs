using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL.Tables
{
    /// <summary>
    /// Impuestos
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class OSTC
    {
        [SAPField(IsSystemField = true)]
        public string Code { get; set; }
        [SAPField(IsSystemField = true)]
        public string Name { get; set; }
        [SAPField(IsSystemField = true)]
        public decimal Rate { get; set; }
    }
}

