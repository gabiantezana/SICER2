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
        [SAPField(IsSystemField = true)]
        public string CardCode { get; set; }

        [SAPField(IsSystemField = true)]
        public string LictradNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string CardName { get; set; }

        [SAPField(IsSystemField = true)]
        public string validFor { get; set; }

        [SAPField(IsSystemField = true)]
        public string CardType { get; set; }
    }

}
