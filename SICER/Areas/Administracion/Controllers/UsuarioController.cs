using SICER.Controllers;
using SICER.EXCEPTION;
using SICER.Filters;
using SICER.HELPER;
using SICER.VIEWMODEL.Administracion.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.LOGIC.Administracion;

namespace SICER.Areas.Administracion.Controllers
{
    public class UsuarioController : BaseController
    {
        #region Get

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.LISTAR)]
        public ActionResult ListUsuarios()
        {
            var model = UsuarioLogic.GetListUsuariosViewModel(GetDataContext(), null, null);
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.LISTAR)]
        public ActionResult _ListUsuariosPaged(ListUsuariosViewModel model, int? page)
        {
            ViewBag.filter = model.Filter;
            model.ListUsuarios = UsuarioLogic.GetUsuariosPagedList(GetDataContext(), model.Filter, page);
            return PartialView("PartialView/_ListUsuariosPartialView", model.ListUsuarios);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.LISTAR)]
        public ActionResult _ListUsuariosPartialView(string filter)
        {
            ViewBag.filter = filter;
            var list = UsuarioLogic.GetUsuariosPagedList(GetDataContext(), filter, null);
            return PartialView("PartialView/_ListUsuariosPartialView", list);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        public ActionResult AddUpdateUsuario(int? usuarioId)
        {
            var model = UsuarioLogic.GetUsuario(GetDataContext(), usuarioId);
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        public ActionResult DisableUsuario(int? usuarioId)
        {
            UsuarioLogic.DisableUsuario(GetDataContext(), usuarioId);
            return RedirectToAction(nameof(ListUsuarios));
        }

        #endregion

        //TODO: SELECT * FROM OIDC - TIPO DOCUMENTOS
        #region Post
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        [HttpPost]
        public ActionResult AddUpdateUsuario(UsuarioViewModel model, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioLogic.AddUpdateUsuario(GetDataContext(), model, formCollection);
                    PostMessage(MessageType.Success);
                    return RedirectToAction(nameof(ListUsuarios));
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

        public JsonResult GetUsuariosJsonList(string filtro)
        {
            var model = UsuarioLogic.GetUsuariosJsonList(GetDataContext(), filtro);
            return Json(model);
        }

        #endregion

    }
}