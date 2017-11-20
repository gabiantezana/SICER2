using System;
using SAPbobsCOM;

namespace SICER.SAPUSERMODEL.Tables
{
    [SAPTable]
    public class MSS_SICER_AACT : DefaultUserTable
    {

        [SAPField(FieldDescription = "Cuenta contable", FieldSize = 200)]
        public string U_MSS_ACC { get; set; }

        [SAPField(FieldDescription = "Descripcion", FieldSize = 200)]
        public string U_MSS_DSC { get; set; }

        [SAPField(FieldDescription = "Es cta de banco",
        FieldSize = 2,
        ValidValues = new[] { "Y", "N" },
        ValidDescription = new[] { "SI", "NO" },
        DefaultValue = "N"
        )]
        public string U_MSS_IBA { get; set; }

    }
}
