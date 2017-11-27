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
    public static class SapConceptoLogic
    {
        public static IEnumerable<@MSS_SICER_CCPT> GetList(DataContext dataContext, string filter)
        {
            return new MSS_SICER_CCPTDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext ,filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.U_MSS_ACC + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.U_MSS_DSC,
            });
        }
    }
}
