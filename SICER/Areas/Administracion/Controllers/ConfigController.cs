using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.Controllers;
using SICER.DATAACCESS.Administracion;
using SICER.EXCEPTION;
using SICER.Filters;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.VIEWMODEL.Administracion.Config;

namespace SICER.Areas.Administracion.Controllers
{
    public class ConfigController : BaseController
    {
        #region Get
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.CONFIG.LISTAR)]
        public ActionResult LstConfigs(Int32? p, LstConfigViewModel model)
        {
            ConfigLogic logic = new ConfigLogic();
            model.LstConfigs = logic.LstConfigs(GetDataContext(), model.CampoBuscar, p);
            model.LstConfigsDefault = new List<MODEL.CONFIG>();
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.CONFIG.CREAR)]
        public ActionResult AddEditConfig(String ConfigID)
        {
            var logic = new ConfigLogic();
            var model = logic.GetConfig(GetDataContext(), ConfigID);

            if (TempData.Get(TempDataKey.ParametrosActiveTab) == null)
                TempData.Set(TempDataKey.ParametrosActiveTab, ActiveTab.InformacionParametros);

            return View(model);
        }


        #endregion
        public String GetConfigValue(String idConfig)
        {
            String value = ConfigLogic.GetCONFIGValue(GetDataContext(), idConfig);
            return value;
        }

        #region PartialViews

        public ActionResult _LstConfigPartialView(String ConfigId, Int32? p)
        {
            var logic = new ConfigLogic();
            var lstModel = logic.GetConfigs(GetDataContext(), ConfigId, p);
            return PartialView("PartialView/_LstConfigPartialView", lstModel);
        }

        public ActionResult _LstConfigsPaged(Int32? p)
        {
            var dataAccess = new ConfigLogic();
            var lstModel = dataAccess.LstConfigs(GetDataContext(), p);
            return (ActionResult)PartialView("PartialView/_LstConfigPartialView", lstModel);
        }
        #endregion

        #region JsonResult
        public JsonResult GetConfigs(String CadenaBuscar)
        {
            var logic = new ConfigLogic();
            var dataConfigs = logic.GetConfigs(GetDataContext(), CadenaBuscar)
                .Select(x => new
                {
                    id = x.id,
                    text = x.text
                });

            return Json(dataConfigs);
            #endregion
        }

        #region Post
        [HttpPost]
        public ActionResult AddEditConfig(ConfigViewModel model)
        {
            var logic = new ConfigLogic();
            TempData.Set(TempDataKey.ParametrosActiveTab, ActiveTab.InformacionParametros);
            if (ModelState.IsValid)
            {
                try
                {
                    var idconfig = logic.SaveConfig(GetDataContext(), model);
                    PostMessage(MessageType.Success);
                    return RedirectToAction("AddEditConfig", new { id = idconfig });
                }
                catch (CustomException ex)
                {
                    PostMessage(ex);
                }
            }

            return View(model);
        }
        #endregion
    }
}