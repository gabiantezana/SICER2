using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL.Tables
{
    /// <summary>
    /// Tipos de cambio
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class ORTT
    {
        [SAPField(IsSystemField = true)]
        public DateTime RateDate { get; set; }
        [SAPField(IsSystemField = true)]
        public string Currency { get; set; }
        [SAPField(IsSystemField = true)]
        public decimal Rate { get; set; }
    }
}

