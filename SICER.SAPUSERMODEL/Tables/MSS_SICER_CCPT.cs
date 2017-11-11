using System;

namespace SICER.SAPUSERMODEL.Tables
{
    public class MSS_SICER_CCPT : DefaultUserTable
    {

        [SAPField(FieldDescription = "Descripcion", FieldSize = 200)]
        public string U_MSS_DSCRPT { get; set; }

        [SAPField(FieldDescription = "Cuenta contable", FieldSize = 200)]
        public string U_MSS_ACCT { get; set; }

    }
}
