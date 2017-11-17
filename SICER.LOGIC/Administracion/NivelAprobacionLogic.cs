using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;

namespace SICER.LOGIC.Administracion
{
    public static class NivelAprobacionLogic
    {
        #region Get List

        private static IEnumerable<NivelAprobacion> GetQueryList(DataContext dataContext, string filter = null)
        {
            return new NivelAprobacionDataAccess(dataContext).GetList(filter);
        }

        public static ListNivelAprobacionViewModel GetList(DataContext dataContext, string query, int? page)
        {
            var model = new ListNivelAprobacionViewModel()
            {
                PagedList = GetPagedList(dataContext, query, page),
                Filter = string.Empty,
                ListDefault = new List<NivelAprobacion>()
            };
            return model;
        }

        public static IPagedList<NivelAprobacion> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetQueryList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

        private static void FillJLists(DataContext dataContext, ref NivelAprobacionViewModel model)
        {
            //Fill select
            model.JListTipoDocumento = TipoDocumentoLogic.GetJList(dataContext);
        }

        #endregion

        #region Get Entity

        public static NivelAprobacionViewModel GetViewModel(DataContext dataContext, int? nivelAprobacionId)
        {
            NivelAprobacion entity = new NivelAprobacionDataAccess(dataContext).GetEntity(nivelAprobacionId);
            return GetViewModelFromEntity(dataContext, entity);
        }

        #endregion

        public static void AddUpdate(DataContext dataContext, NivelAprobacionViewModel model)
        {

            ValidarDuplicado(dataContext, model);
            new NivelAprobacionDataAccess(dataContext).AddUpdate(model);
            return;
        }

        private static NivelAprobacionViewModel GetViewModelFromEntity(DataContext dataContext, NivelAprobacion entity)
        {
            NivelAprobacionViewModel model = entity?.ConvertTo(typeof(NivelAprobacionViewModel));
            model = model ?? new NivelAprobacionViewModel();

            FillJLists(dataContext, ref model);
            return model;
        }

        private static void ValidarDuplicado(DataContext dataContext, NivelAprobacionViewModel model)
        {
            //TODO:
        }

        public static int GetFirstNivelAprobacion(DataContext dataContext, DocumentType documentType)
        {
            return GetNumeroNivelAprobacion(dataContext, documentType);
        }

        public static int GetLastNivelAprobacion(DataContext dataContext, DocumentType documentType)
        {
            return GetNumeroNivelAprobacion(dataContext, documentType, true);
        }

        private static int GetNumeroNivelAprobacion(DataContext dataContext, DocumentType documentType, bool getLast = false)
        {
            var list = dataContext.Context.NivelAprobacion.Where(x => x.TipoDocumentoId == (int)documentType).Select(y => y.NumeroNivel).ToList();

            list = getLast ? list.OrderByDescending(x => x).ToList() : list.OrderBy(x => x).ToList();
            var firstNivelAprobacion = list.FirstOrDefault();
            if (firstNivelAprobacion == 0)
                throw new Exception("No se encuentra ningún nivel de aprobación para el documento " + documentType.ToString());
            return firstNivelAprobacion;
        }

        /// <summary>
        /// Filtrar listado select2
        /// </summary>
        public static IEnumerable<JsonEntity> GetFilterJsonList(DataContext dataContext, string filtro)
        {
            //TODO:
            var query = GetQueryList(dataContext);
            if (!string.IsNullOrEmpty(filtro))
            {
                /*
                query = filtro.ToLower().Split(' ').Aggregate(query,
                    (current, token) => current.Where(x => x.Apellidos.ToLower().Contains(token)
                                         || x.Nombres.ToLower().Contains(token)
                                         || x.UserName.ToLower().Contains(token)));*/
            }
            var jsonEntities = query?.Select(x => new JsonEntity()
            {/*
                id = x.UsuarioId,
                text = x.GetNombreCompleto(),*/
            });

            return jsonEntities;
        }
    }
}
