using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.LOGIC.Sync
{
    public static class SapCentroCostoLogic
    {
        public static IEnumerable<OOCR> GetList(DataContext dataContext, int? dimCode, string filter)
        {
            return new OocrDataAccess(dataContext).GetList(dimCode, filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter, int? dimCode)
        {
            return GetList(dataContext, dimCode, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.OcrCode,
                text = x.OcrCode + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.OcrName,
            });
        }
    }
}
