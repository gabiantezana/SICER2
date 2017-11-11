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
        public DateTime RateDate { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}

