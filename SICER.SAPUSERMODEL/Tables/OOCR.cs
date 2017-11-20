using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL.Tables
{
    /// <summary>
    /// Sap Centros costo
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class OOCR
    {
        [SAPField(IsSystemField = true)]
        public string OcrCode { get; set; }
        [SAPField(IsSystemField = true)]
        public string OcrName { get; set; }
        [SAPField(IsSystemField = true)]
        public int DimCode { get; set; }
        [SAPField(IsSystemField = true)]
        public string Locked { get; set; }
    }
}
