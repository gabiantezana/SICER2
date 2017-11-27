using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.Controllers;
using SICER.DATAACCESS.Sync;
using SICER.Filters;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.LOGIC.Sync;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;

namespace SICER.Areas.Sync.Controllers
{
    public class SapBusinessPartnerController : BaseController
    {
        /// <summary>
        /// Fill select2
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetJList(string filter)
        {
            var model = SapBusinessPartnerLogic.GetJList(GetDataContext(), filter);
            return Json(model);
        }
    }
}