using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using SICER.Controllers;
using SICER.DATAACCESS;
using SICER.DATAACCESS.Sync;
using SICER.EXCEPTION;
using SICER.LOGIC;
using SICER.LOGIC.Sync;
using SICER.MODEL;

namespace SICER.Areas.Administracion.Controllers
{
    public class SapController : BaseController
    {
        // GET: Administracion/Sap
        public JsonResult GenerateDbSchema()
        {
            try
            {

                var message = SapLogic.GenerateSapDbSchema(GetDataContext().GetAndConnectCurrentCompany());
                return Json(true);
            }
            catch (SapException ex)
            {
                ex.Message = new SapLogic().GetLastError(GetDataContext().GetAndConnectCurrentCompany());
                throw ex;
            }
        }

        public JsonResult StartSyncronization()
        {
            BackgroundJob.Enqueue(() => SyncBusinessPartner());

            return Json(true);
        }

        public void SyncBusinessPartner()
        {
            while (true)
            {
                new SyncLogic(GetDataContext()).SyncAll();

            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}