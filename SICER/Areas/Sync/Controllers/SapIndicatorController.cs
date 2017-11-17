using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.Controllers;
using SICER.DATAACCESS.Sync;
using SICER.LOGIC.Sync;
using SICER.VIEWMODEL.Sync;

namespace SICER.Areas.Sync.Controllers
{
    public class SapIndicatorController : BaseController
    {
        /// <summary>
        /// Vista principal
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var model = SapIndicatorLogic.GetListViewModel(GetDataContext());
            return View(model);
        }

        /// <summary>
        /// Lista paginada
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult _PagedList(ListSapIndicatorViewModel model, string filter, int? page)
        {
            ViewBag.filter = model.Filter;
            model.PagedList = SapIndicatorLogic.GetPagedList(GetDataContext(), filter, page);
            return PartialView("PartialView/_PagedList", model.PagedList);
        }

        /// <summary>
        /// Lista filtrada
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult _FilterList(string filter)
        {
            ViewBag.filter = filter;
            var list = SapIndicatorLogic.GetPagedList(GetDataContext(), filter, null);
            return PartialView("PartialView/_PagedList", list);
        }

        /// <summary>
        /// Fill select2
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetJList(string filter)
        {
            var model = SapIndicatorLogic.GetJList(GetDataContext(), filter);
            return Json(model);
        }
    }
}