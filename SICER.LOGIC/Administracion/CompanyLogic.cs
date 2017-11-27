using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.DATAACCESS.GestionDocumentos;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Area;
using SICER.VIEWMODEL.GestionDocumentos;

namespace SICER.LOGIC.Administracion
{
    public static class CompanyLogic
    {
        #region GetList

        private static IEnumerable<Company> GetQuery(DataContext dataContext, string filter = null)
        {
            return new CompanyDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntity> GetJList(DataContext dataContext, string filter)
        {
            var query = GetQuery(dataContext, filter);
            var jsonEntities = query?.Select(x => new JsonEntity()
            {
                id = x.CompanyId,
                text = x.DbName,
            });
            return jsonEntities;
        }

        #endregion
    }
}
