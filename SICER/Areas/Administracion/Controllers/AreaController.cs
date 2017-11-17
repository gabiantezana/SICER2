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
using SICER.VIEWMODEL.Administracion.Area;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;
using SICER.VIEWMODEL.Administracion.Usuario;

namespace SICER.Areas.Administracion.Controllers
{
    public class AreaController : BaseController
    {
        #region Get

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Area.LISTAR)]
        public ActionResult List()
        {
            var model = AreaLogic.GetList(GetDataContext(), null, null);
            return View(model);
        }

        /// <summary>
        /// Lista paginada
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Area.LISTAR)]
        public ActionResult _PagedList(ListAreaViewModel model, int? page)
        {
            ViewBag.filter = model.Filter;
            model.PagedList = AreaLogic.GetPagedList(GetDataContext(), model.Filter, page);
            return PartialView("PartialView/_PagedList", model.PagedList);
        }

        /// <summary>
        /// Lista filtrada
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Area.LISTAR)]
        public ActionResult _FilterList(string filter)
        {
            ViewBag.filter = filter;
            var list = AreaLogic.GetPagedList(GetDataContext(), filter, null);
            return PartialView("PartialView/_PagedList", list);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Area.CREAR)]
        public ActionResult AddUpdate(int? areaId)
        {
            var model = AreaLogic.GetViewModel(GetDataContext(), areaId);
            return View(model);
        }

        #endregion

        #region Post
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Area.CREAR)]
        [HttpPost]
        public ActionResult AddUpdate(AreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AreaLogic.AddUpdate(GetDataContext(), model);
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

        /// <summary>
        /// Only for filter in select2
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonList(string filter)
        {
            var list = AreaLogic.GetJList(GetDataContext(), filter);
            return Json(list);
        }

        #endregion
    }
}