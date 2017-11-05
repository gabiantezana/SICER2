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
            var model = UsuarioLogic.GetList(GetDataContext());
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        public ActionResult AddUpdateUsuario(int? usuarioId)
        {
            var model = UsuarioLogic.GetUsuario(GetDataContext(), usuarioId);
            return View(model);
        }

        #endregion

        #region Post
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.CREAR)]
        [HttpPost]
        public ActionResult AddUpdateUsuario(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioLogic.AddUpdateUsuario(GetDataContext(), model);
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

    }
}