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
        public string OcrCode { get; set; }
        public string OcrName { get; set; }
        public int DimCode { get; set; }
        public string Locked { get; set; }
    }
}
