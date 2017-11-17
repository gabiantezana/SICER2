using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.Controllers;
using SICER.EXCEPTION;
using SICER.Filters;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;
using SICER.VIEWMODEL.Administracion.Usuario;

namespace SICER.Areas.Administracion.Controllers
{
    public class NivelAprobacionController : BaseController
    {
        #region Get

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.NivelAprobacion.LISTAR)]
        public ActionResult List()
        {
            var model = NivelAprobacionLogic.GetList(GetDataContext(), null, null);
            return View(model);
        }

        /// <summary>
        /// Lista paginada
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.NivelAprobacion.LISTAR)]
        public ActionResult _PagedList(ListNivelAprobacionViewModel model, int? page)
        {
            ViewBag.filter = model.Filter;
            model.PagedList = NivelAprobacionLogic.GetPagedList(GetDataContext(), model.Filter, page);
            return PartialView("PartialView/_PagedList", model.PagedList);
        }

        /// <summary>
        /// Lista filtrada
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.LISTAR)]
        public ActionResult _FilterList(string filter)
        {
            ViewBag.filter = filter;
            var list = NivelAprobacionLogic.GetPagedList(GetDataContext(), filter, null);
            return PartialView("PartialView/_PagedList", list);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        public ActionResult AddUpdate(int? nivelAprobacionId)
        {
            var model = NivelAprobacionLogic.GetViewModel(GetDataContext(), nivelAprobacionId);
            return View(model);
        }

        #endregion

        //TODO: SELECT * FROM OIDC - TIPO DOCUMENTOS
        #region Post
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        [HttpPost]
        public ActionResult AddUpdate(NivelAprobacionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NivelAprobacionLogic.AddUpdate(GetDataContext(), model);
                    PostMessage(MessageType.Success);
                    return RedirectToAction(nameof(List));
                }
                catch (CustomException ex)
                {
                    PostMessage(ex);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            }
            return View(model);
        }

        #endregion

        #region JsonResult


        #endregion

        /// <summary>
        /// Only for filter in select2
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonList()
        {
            //TODO: IMPLEMENT FILTERS
            var model = new List<JsonEntity>();
            return Json(model);
        }
    }
}