using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.SAPUSERMODEL.Tables
{
    /// <summary>
    /// Sap Business Partners
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class OCRD
    {
        public string CardCode { get; set; }
        public string LictradNum { get; set; }
        public string CardName { get; set; }
        public string validFor { get; set; }
        public string CardType { get; set; }
    }

}
