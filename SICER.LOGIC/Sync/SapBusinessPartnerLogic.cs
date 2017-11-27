using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.LOGIC.Sync
{
    public static class SapBusinessPartnerLogic
    {
        public static IEnumerable<OCRD> GetList(DataContext dataContext, string filter)
        {
            return new OcrdDataAccess(dataContext).GetList(filter).ToList();
        }

        public static List<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.CardCode,
                text = x.LictradNum + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CardName,
            }).ToList();
        }
    }
}
