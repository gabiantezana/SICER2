using SICER.Controllers;
using SICER.DATAACCESS.Administracion;
using SICER.Filters;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SICER.Areas.Administracion.Controllers
{
    [AppRolAuthorize(AppRol.SuperAdministrador)]
    public class RolController : BaseController
    {
        public ActionResult LstRoles()
        {
            var model = new RolDataAccess().LstRoles(GetDataContext());
            return View(model);
        }

        public ActionResult EditPermisosPorRoles(Int32 idRol)
        {
            var model = new RolDataAccess().EditPermisosPorRoles(GetDataContext(), idRol);
            return View(model);
        }

        public ActionResult AddEditRol(Int32? RolId)
        {
            var model = new RolDataAccess().GetRol(GetDataContext(), RolId);
            return View(model);
        }

        [HttpPost]
        public ActionResult LstRoles(LstRolUsuarioViewModel model, FormCollection collection)
        {
            try
            {
                RolDataAccess dataAccess = new RolDataAccess();

                var rolId = model.idRol;
                List<VistasRol> vistasRoles = context.VistasRol.Where(x => x.idRol == rolId).ToList();


                using (var transaction = new TransactionScope())
                {
                    foreach (var vistaRol in vistasRoles)
                    {
                        vistaRol.estado = false;
                        context.SaveChanges();
                    }
                    transaction.Complete();
                }
                var vistasUsariosKey = collection.AllKeys.Where(x => x.StartsWith("chk-"));

                using (var transaction = new TransactionScope())
                {
                    foreach (var vistaUsuarioKey in vistasUsariosKey)
                    {
                        var value = collection[vistaUsuarioKey.ToString()] == "on" || collection[vistaUsuarioKey.ToString()] == "true" ? true : false;

                        var vistaNombre = vistaUsuarioKey.Split('-')[1];
                        VistasRol vistaRol = context.VistasRol.FirstOrDefault(x => x.idRol == rolId && x.Vistas.nombre.Equals(vistaNombre));

                        if (vistaRol == null)
                        {
                            // Vistas vista = context.VistasRol.FirstOrDefault(x => x.Vistas.nombre.Equals(vistaNombre)).Vistas;
                            Vistas vista = context.Vistas.FirstOrDefault(x => x.nombre.Equals(vistaNombre));

                            vistaRol = new VistasRol();
                            vistaRol.idRol = rolId;
                            vistaRol.idVista = vista.idVista;
                            vistaRol.estado = value;
                            context.VistasRol.Add(vistaRol);
                        }
                        else
                        {
                            vistaRol.estado = value;
                        }
                        context.SaveChanges();

                    }
                    transaction.Complete();
                    PostMessage(MessageType.Success);
                }
            }
            catch (Exception ex)
            {
                PostMessage(MessageType.Error, "Ocurrio un error inesperado. " + ex.ToString());
            }

            return RedirectToAction("LstRoles");
        }

    }
}