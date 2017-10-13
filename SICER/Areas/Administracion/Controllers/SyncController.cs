using SICER.Controllers;
using SICER.DATAACCESS.Administracion;
using SICER.Filters;
using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICER.Areas.Administracion.Controllers
{
    [AppRolAuthorize(AppRol.SuperAdministrador)]
    public class SyncController : BaseController
    {
        public ActionResult ListSAPCentrosCosto()
        {
            var model = new SyncDataAccess().ListSAPCentrosCosto(GetDataContext());
            return View(model);
        }

        public ActionResult ListSAPMetodoPago()
        {
            var model = new SyncDataAccess().ListSAPMetodoPago(GetDataContext());
            return View(model);
        }

        public ActionResult ListSAPMoneda()
        {
            var model = new SyncDataAccess().ListSAPMoneda(GetDataContext());
            return View(model);
        }

        public ActionResult ListSAPProveedor()
        {
            var model = new SyncDataAccess().ListSAPProveedor(GetDataContext());
            return View(model);
        }

        public ActionResult ListSAPServicio()
        {
            var model = new SyncDataAccess().ListSAPServicio(GetDataContext());
            return View(model);
        }
    }
}