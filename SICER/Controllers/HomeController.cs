using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SICER.DATAACCESS.Administracion;

namespace SICER.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            new SyncDataAccess().SyncTables(GetDataContext());
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                //-----------------------TEST---------------------------
                /*
                Usuario _usuario = GetDataContext().Context.Usuario.Include(x => x.Rol.VistasRol.Select(y => y.Vistas)).FirstOrDefault(x => x.username == model.Codigo);
                Session.Set(SessionKey.NombresUsuario, _usuario.nombres);
                Session.Set(SessionKey.UserName, _usuario.username);
                Session.Set(SessionKey.IdUsuario, _usuario.idUsuario);
                Session.Set(SessionKey.Rol, (AppRol)Enum.Parse(typeof(AppRol), _usuario.Rol.nombre));
                Session.Set(SessionKey.Vistas, context.Vistas.Select(x => x.nombre).ToArray());
                return RedirectToAction(nameof(this.ChangePassword));*/
                //-----------------------TEST---------------------------


                Usuario usuario = GetDataContext().Context.Usuario.Include(x => x.Rol.VistasRol.Select(y => y.Vistas)).FirstOrDefault(x => x.username == model.Codigo);

                if (usuario != null)
                {
                    var byteArrayPassword = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);

                    if (!byteArrayPassword.SequenceEqual(usuario.password))
                    {
                        PostMessage(MessageType.Error, "Contraseña incorrecta");
                        return RedirectToAction(nameof(this.Login));
                    }
                    else if (!(usuario.estado ?? false))
                    {
                        PostMessage(MessageType.Error, "Su usuario no se encuentra activo");
                        return RedirectToAction(nameof(this.Login));
                    }
                    else
                    {
                        Session.Set(SessionKey.NombresUsuario, usuario.nombres);
                        Session.Set(SessionKey.UserName, usuario.username);
                        Session.Set(SessionKey.IdUsuario, usuario.idUsuario);
                        Session.Set(SessionKey.Rol, (AppRol)Enum.Parse(typeof(AppRol), usuario.Rol.nombre));
                        Session.Set(SessionKey.Vistas, usuario.Rol.VistasRol.Where(x => x.estado == true).Select(x => x.Vistas.valor).ToArray());

                        switch (Session.GetRol())
                        {
                            case AppRol.SuperAdministrador:
                                Session.Set(SessionKey.Vistas, context.Vistas.Select(x => x.valor).ToArray());
                                break;
                        }

                        var DefaultEncryptPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.DEFAULT_PASSWORD, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                        if (DefaultEncryptPassword.SequenceEqual(usuario.password))
                            return RedirectToAction(nameof(this.ChangePassword));
                        else
                        {
                            return RedirectToAction(nameof(this.Index));
                        }
                    }
                }
                else
                {
                    PostMessage(MessageType.Error, "Su usuario no existe");
                    return RedirectToAction(nameof(this.Login));
                }
            }
            catch (Exception ex)
            {
                PostMessage(MessageType.Error, "Ocurrió un error inesperado: " + ex.ToString());

            }
            return RedirectToAction(nameof(this.Login));
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction(nameof(this.Login));
        }


        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                UsuarioDataAccess dataAccess = new UsuarioDataAccess();
                dataAccess.ChangePassword(GetDataContext(), model);
                PostMessage(MessageType.Success, "Su contraseña se cambio exitosamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session.Clear();
                PostMessage(MessageType.Error, ex.ToString());
                return RedirectToAction(nameof(this.Login));
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}