using System;

namespace SICER.SAPUSERMODEL.Test.Conductor
{
    //[DBStructure]
    //[SAPTable("Tabla conductor-direcciones", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)]
    public static class MSS_CDTR_DIR 
    {
        [SAPField(FieldDescription ="Dirección completa")]
        public static String MSS_DIRECCION { get; set; }

        [SAPField(FieldDescription ="País")]
        public static String MSS_PAIS { get; set; }
    }
}
