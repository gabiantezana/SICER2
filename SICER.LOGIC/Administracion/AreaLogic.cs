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
    public static class AreaLogic
    {
        #region GetList

        public static ListAreaViewModel GetList(DataContext dataContext, string query, int? page)
        {
            var model = new ListAreaViewModel()
            {
                PagedList = GetPagedList(dataContext, query, page),
                Filter = string.Empty,
                ListDefault = new List<Documento>(),
            };
            return model;
        }

        private static IEnumerable<Area> GetQuery(DataContext dataContext, string filter = null)
        {
            return new AreaDataAccess(dataContext).GetList(filter);
        }

        public static IPagedList<Area> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetQuery(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

        public static IEnumerable<JsonEntity> GetJList(DataContext dataContext,string filter)
        {
            //TODO:
            var query = GetQuery(dataContext, filter);
            var jsonEntities = query?.Select(x => new JsonEntity()
            {
                id = x.AreaId,
                text = x.Descripcion,
            });

            return jsonEntities;
        }

        /// <summary>
        /// Filtrar listado select2
        /// </summary>

        #endregion

        #region Get Entity

        public static AreaViewModel GetViewModel(DataContext dataContext, int? areaId)
        {
            var area = new AreaDataAccess(dataContext).GetEntity(areaId);
            var model = area.ConvertTo(typeof(AreaViewModel));
            return model;
        }

        #endregion
        public static void AddUpdate(DataContext dataContext, AreaViewModel model)
        {
            new AreaDataAccess(dataContext).AddUpdate(model);
        }

    }
}
