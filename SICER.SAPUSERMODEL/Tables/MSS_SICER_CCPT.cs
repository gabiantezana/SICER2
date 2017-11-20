using System;

namespace SICER.SAPUSERMODEL.Tables
{
    [SAPTable]
    public class MSS_SICER_CCPT : DefaultUserTable
    {

        [SAPField(FieldDescription = "Descripcion", FieldSize = 200)]
        public string U_MSS_DSC { get; set; }

        [SAPField(FieldDescription = "Cuenta contable", FieldSize = 200)]
        public string U_MSS_ACC { get; set; }

    }
}
