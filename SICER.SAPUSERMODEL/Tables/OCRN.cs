using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL.Tables
{
    /// <summary>
    /// Sap Monedas
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class OCRN
    {
        public string DocCurrCod { get; set; }
        public string CurrName { get; set; }
        public string Locked { get; set; }
    }
}
