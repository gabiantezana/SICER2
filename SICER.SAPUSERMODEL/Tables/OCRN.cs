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
        [SAPField(IsSystemField = true)]
        public string DocCurrCod { get; set; }
        [SAPField(IsSystemField = true)]
        public string CurrName { get; set; }
        [SAPField(IsSystemField = true)]
        public string Locked { get; set; }
    }
}
