﻿using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SICER.DATAACCESS.Administracion;
using SICER.LOGIC.Administracion;

namespace SICER.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //new SyncDataAccess().SyncTables(GetDataContext());
            return View();
        }

        public ActionResult Login()
        {
            var model = new LoginViewModel()
            {
                CompanyJList = CompanyLogic.GetJList(GetDataContext(), null)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                //-----------------------TEST---------------------------

                /*Usuario _usuario = GetDataContext().Context.Usuario.Include(x => x.Rol.VistaRol.Select(y => y.Vista)).FirstOrDefault(x => x.UserName == model.Codigo);
                Session.Set(SessionKey.NombresUsuario, _usuario.Nombres);
                Session.Set(SessionKey.IdUsuario, _usuario.UsuarioId);
                Session.Set(SessionKey.Rol, (AppRol)Enum.Parse(typeof(AppRol), _usuario.Rol.Codigo));
                Session.Set(SessionKey.Vistas, context.Vista.Select(x => x.Codigo).ToArray());
                return RedirectToAction(nameof(ChangePassword));*/
                //-----------------------TEST---------------------------

                Usuario usuario = GetDataContext().Context.Usuario.Include(x => x.Rol.VistaRol.Select(y => y.Vista)).FirstOrDefault(x => x.UserName == model.Codigo);
                if (usuario != null)
                {
                    if (usuario.Rol.Codigo != AppRol.SUPERADMIN.ToString()
                       && usuario.Rol.Codigo != AppRol.SISTEMAS.ToString()
                       )
                    {
                        usuario = GetDataContext().Context.Usuario.Include(x => x.Rol.VistaRol.Select(y => y.Vista))
                            .FirstOrDefault(x => x.UserName == model.Codigo && x.CompanyId == model.CompanyId);
                        if (usuario == null)
                        {
                            PostMessage(MessageType.Error, "Su usuario no tiene permiso para esta compañía.");
                            return RedirectToAction(nameof(this.Login));
                        }

                    }

                    var byteArrayPassword = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                    if (!byteArrayPassword.SequenceEqual(usuario.Password))
                    {
                        PostMessage(MessageType.Error, "Contraseña incorrecta");
                        return RedirectToAction(nameof(this.Login));
                    }
                    if (!usuario.Estado)
                    {
                        PostMessage(MessageType.Error, "Su usuario no se encuentra activo");
                        return RedirectToAction(nameof(this.Login));
                    }

                    Session.Set(SessionKey.NombresUsuario, usuario.Nombres + " " + usuario.Apellidos);
                    Session.Set(SessionKey.UserName, usuario.UserName);
                    Session.Set(SessionKey.UsuarioId, usuario.UsuarioId);
                    Session.Set(SessionKey.CompanyName, usuario.Company?.DbName);
                    Session.Set(SessionKey.CompanyId, usuario.CompanyId);

                    if (usuario.Rol.Codigo == ConstantHelper.CODIGOROLSUPERADMINISTRADOR)
                    {
                        Session.Set(SessionKey.Rol, AppRol.SUPERADMIN);
                        Session.Set(SessionKey.Vistas, GetDataContext().Context.Vista.Select(x => x.Codigo).ToArray());
                    }
                    else
                    {
                        Session.Set(SessionKey.Rol, Enum.Parse(typeof(AppRol), usuario.Rol.Codigo));
                        Session.Set(SessionKey.Vistas, usuario.Rol.VistaRol.Where(x => x.Estado).Select(x => x.Vista.Codigo).ToArray());
                        if (usuario.Rol.Codigo == ConstantHelper.CODIGOROLGESTORDOCUMENTOS)
                        {
                            var vistasDeRolesUsuario = GetDataContext().Context.UsuarioRoles
                                 .Where(x => x.UsuarioId == usuario.UsuarioId && x.Estado).SelectMany(x => x.Rol.VistaRol).Select(y => y.Vista.Codigo).ToArray();

                            Session.Set(SessionKey.Vistas, vistasDeRolesUsuario);
                        }
                    }

                    var defaultEncryptPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.PASSWORD_DEFAULT, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                    return RedirectToAction(defaultEncryptPassword.SequenceEqual(usuario.Password) ? nameof(this.ChangePassword) : nameof(this.Index));
                }

                PostMessage(MessageType.Error, "Su usuario no existe");
                return RedirectToAction(nameof(this.Login));
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

        public ContentResult KeepAlive()
        {
            return Content(String.Empty);
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
                UsuarioLogic.ChangePassword(GetDataContext(), model);

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

        public ActionResult PermisoInsuficiente()
        {
            return View();
        }
    }
}