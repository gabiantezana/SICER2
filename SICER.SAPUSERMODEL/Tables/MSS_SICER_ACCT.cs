using System;
using SAPbobsCOM;

namespace SICER.SAPUSERMODEL.Tables
{
    public class MSS_SICER_AACT : DefaultUserTable
    {
        [SAPField(FieldDescription = "Descripcion", FieldSize = 200)]
        public string U_MSS_DSCRPT { get; set; }

        [SAPField(FieldDescription = "Es cta de banco",
        FieldSize = 2,
        ValidValues = new[] { "Y", "N" },
        ValidDescription = new[] { "SI", "NO" },
        DefaultValue = "N"
        )]
        public string U_MSS_IBACCT { get; set; }

        [SAPField(FieldDescription = "Cuenta contable", FieldSize = 200)]
        public string U_MSS_ACCT { get; set; }

    }
}
