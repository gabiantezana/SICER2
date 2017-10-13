using SICER.Controllers;
using SICER.DATAACCESS.Administracion;
using SICER.EXCEPTION;
using SICER.Filters;
using SICER.HELPER;
using SICER.VIEWMODEL.Administracion.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICER.Areas.Administracion.Controllers
{
    public class UsuarioController : BaseController
    {
        #region Get

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.LISTAR)]
        public ActionResult LstUsuarios()
        {
            var model = new UsuarioDataAccess().LstUsuarios(GetDataContext());
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.EDITAR)]
        public ActionResult AddUpdateUsuario(Int32? UsuarioID)
        {
            var usuario = new UsuarioDataAccess().GetUsuario(GetDataContext(), UsuarioID);
            return View(usuario);
        }

        #endregion

        #region Post
        [AppViewAuthorize(ConstantHelper.Vistas.Administracion.Usuario.EDITAR)]
        public ActionResult AddUpdateUsuario(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new UsuarioDataAccess().AddUpdateUsuario(GetDataContext(), model);
                    PostMessage(MessageType.Success);
                    return RedirectToAction(nameof(LstUsuarios));
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