using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.LOGIC.Sync
{
    public class SapMonedaLogic
    {
        public static IEnumerable<OCRN> GetList(DataContext dataContext, string filter)
        {
            return new OcrnDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.DocCurrCod,
                text = x.DocCurrCod + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CurrName
            });
        }

    }
}
