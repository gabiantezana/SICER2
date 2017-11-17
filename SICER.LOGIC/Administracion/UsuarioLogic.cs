using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.DATAACCESS.Sync;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.LOGIC.Sync;
using SICER.VIEWMODEL.Administracion.Usuario;
using SICER.VIEWMODEL.General;
using SICER.MODEL;

namespace SICER.LOGIC.Administracion
{
    public static class UsuarioLogic
    {
        private static IEnumerable<Usuario> GetQueryUsuariosList(DataContext dataContext, string filtro = null, int? usuarioId = null)
        {
            return new UsuarioDataAccess(dataContext).GetList(filtro, usuarioId);
        }

        public static ListUsuariosViewModel GetListUsuariosViewModel(DataContext dataContext, string query, int? page)
        {
            var model = new ListUsuariosViewModel()
            {
                ListUsuarios = GetUsuariosPagedList(dataContext, query, page),
                Filter = string.Empty,
                ListUsuariosDefault = new List<Usuario>()
            };
            return model;
        }

        public static IPagedList<Usuario> GetUsuariosPagedList(DataContext dataContext, string query, int? page)
        {
            return GetQueryUsuariosList(dataContext, query).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

        private static void FillJLists(DataContext dataContext, ref UsuarioViewModel model)
        {
            //Fill select
            model.RolJList = RolLogic.GetJList(dataContext, RolNivel.Principal);

            //Fill all secundary roles
            model.RolList = new RolDataAccess(dataContext).GetList(RolNivel.Secundario);

            //Fill all user roles
            model.RolUserList = new UsuarioDataAccess(dataContext).GetUsuarioRolesList(model.UsuarioId);

            //Fill proveedor asociado
            if (!string.IsNullOrEmpty(model.SapBusinessPartnerCardCode))
                model.SapBusinessPartnerJList = SapBusinessPartnerLogic.GetJList(dataContext, model.SapBusinessPartnerCardCode);
            //Fil áreas
            model.AreaJList = AreaLogic.GetJList(dataContext, null);

            //Fill list int of Niveles de aprobación
            var usuarioId = model.UsuarioId;
            model.NivelAprobacionIdList = dataContext.Context.UsuarioNivelAprobacion
                .Where(x => x.UsuarioId == usuarioId).Select(x => x.NivelAprobacionId).ToList();
        }

        public static UsuarioViewModel GetUsuario(DataContext dataContext, int? usuarioId)
        {
            Usuario usuario = new UsuarioDataAccess(dataContext).GetUsuario(usuarioId);
            return GetUsuarioViewModel(dataContext, usuario);
        }

        public static void AddUpdateUsuario(DataContext dataContext, UsuarioViewModel model, FormCollection formCollection)
        {

            ValidarDuplicado(dataContext, model);

            model.RolUserList = new List<UsuarioRoles>();
            var rolesUsuarioKey = formCollection.AllKeys.Where(x => x.StartsWith("chk-"));
            var nivelesAprobacionIds = formCollection.AllKeys.Where(x => x.StartsWith("chkNivelAprobacion-"));
            var listadoRolesActivos = new List<Rol>();
            var listadoNivelesDeAprobacion = new List<int>();

            if (model.RolId == (int)AppRol.GESTORDOCUMENTOS)
            {
                foreach (var rolUsuarioKey in rolesUsuarioKey)
                {
                    var value = formCollection[rolesUsuarioKey.ToString()] == "on" || formCollection[rolesUsuarioKey.ToString()] == "true";
                    var rolCodigo = rolUsuarioKey.Split('-')[1];

                    var rol = new RolDataAccess(dataContext).GetList(RolNivel.Secundario).FirstOrDefault(x => x.Codigo == rolCodigo);
                    listadoRolesActivos.Add(rol);
                }
                foreach (var nivelAprobacionKey in nivelesAprobacionIds)
                {
                    var value = formCollection[nivelesAprobacionIds.ToString()] == "on" || formCollection[nivelesAprobacionIds.ToString()] == "true";
                    var nivelAprobacionId = nivelAprobacionKey.Split('-')[1];
                    listadoNivelesDeAprobacion.Add(Convert.ToInt32(nivelAprobacionId));
                }
            }
            model.RolList = listadoRolesActivos;
            model.NivelAprobacionIdList = listadoNivelesDeAprobacion;
            new UsuarioDataAccess(dataContext).AddUpdateUsuario(model);
        }

        public static void ChangePassword(DataContext dataContext, ChangePasswordViewModel model)
        {
            new UsuarioDataAccess(dataContext).ChangePassword(model);
        }

        private static UsuarioViewModel GetUsuarioViewModel(DataContext dataContext, Usuario usuario)
        {
            UsuarioViewModel model = usuario?.ConvertTo(typeof(UsuarioViewModel));
            model = model ?? new UsuarioViewModel();

            FillJLists(dataContext, ref model);
            return model;
        }

        private static void ValidarDuplicado(DataContext dataContext, UsuarioViewModel model)
        {
            const string message = "El campo UserName* ya existe para otro usuario.";
            var usuario = dataContext.Context.Usuario.FirstOrDefault(x => x.UserName == model.UserName
                                                                         && x.UsuarioId != model.UsuarioId
                                                                         );
            if (usuario != null)
                throw new CustomException(new TempDataEntityException { Mensaje = message, TipoMensaje = MessageTypeException.Warning }, dataContext);
        }

        private static UsuarioRolesViewModel GetUsuarioRolViewModel(DataContext dataContext, UsuarioRoles usuarioRoles)
        {
            UsuarioRolesViewModel model = usuarioRoles?.ConvertTo(typeof(UsuarioRolesViewModel));
            model = model ?? new UsuarioRolesViewModel();
            return model;
        }

        public static void DisableUsuario(DataContext dataContext, int? usuarioId)
        {
            new UsuarioDataAccess(dataContext).DisableUsuario(usuarioId);
        }

        /// <summary>
        /// Filtrar listado de usuarios por nombres y usuario
        /// </summary>
        public static IEnumerable<JsonEntity> GetUsuariosJsonList(DataContext dataContext, string filtro)
        {
            var query = GetQueryUsuariosList(dataContext);
            if (!string.IsNullOrEmpty(filtro))
            {
                query = filtro.ToLower().Split(' ').Aggregate(query,
                    (current, token) => current.Where(x => x.Apellidos.ToLower().Contains(token)
                                         || x.Nombres.ToLower().Contains(token)
                                         || x.UserName.ToLower().Contains(token)));
            }
            var jsonEntities = query?.Select(x => new JsonEntity()
            {
                id = x.UsuarioId,
                text = x.GetNombreCompleto(),
            });

            return jsonEntities;
        }

        public static IEnumerable<JsonEntity> GetUsuariosJsonList(DataContext dataContext, int? usuarioId)
        {
            var query = GetQueryUsuariosList(dataContext).Where(x=> x.UsuarioId == usuarioId);
            var jsonEntities = query?.Select(x => new JsonEntity()
            {
                id = x.UsuarioId,
                text = x.GetNombreCompleto(),
            });

            return jsonEntities;
        }

    }

}

