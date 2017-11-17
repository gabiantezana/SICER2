using SICER.VIEWMODEL.Administracion.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Transactions;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.General;

namespace SICER.DATAACCESS.Administracion
{
    public class UsuarioDataAccess
    {
        private DataContext DataContext { get; set; }
        public UsuarioDataAccess(DataContext dataContext) { DataContext = dataContext; }

        private IQueryable<Usuario> QueryUsuarios(string filtro, int? usuarioId = null)
        {
            var query = DataContext.Context.Usuario.AsQueryable();
            if (usuarioId.HasValue)
                query = query.Where(x => x.UsuarioId == usuarioId);

            else if (!string.IsNullOrEmpty(filtro))
            {
                return filtro.ToLower().Split(' ').Aggregate(query, (current, token) =>
                current.Where(x => x.Nombres.ToLower().Contains(token)
                                || x.Apellidos.ToLower().Contains(token)
                                || x.UserName.ToLower().Contains(token)
                                || x.Documento.Contains(filtro)
                                || x.Correo.Contains(filtro)
                                || x.SapBusinessPartner.CardName.Contains(filtro)
                                || x.SapBusinessPartner.SapBusinessPartnerCardCode.Contains(filtro)
                    ));
            }
            return query;
        }

        public IEnumerable<Usuario> GetList(string filter, int? usuarioId)
        {
            return QueryUsuarios(filter, usuarioId);
        }

        public IEnumerable<UsuarioRoles> GetUsuarioRolesList(int? usuarioId)
        {
            return DataContext.Context.UsuarioRoles.Where(x => x.UsuarioId == usuarioId).ToList();
        }

        public Usuario GetUsuario(int? usuarioId)
        {
            return DataContext.Context.Usuario.Find(usuarioId);
        }

        public void AddUpdateUsuario(UsuarioViewModel model)
        {
            Usuario usuario = DataContext.Context.Usuario.Find(model.UsuarioId);
            bool isUpdate = usuario != null;

            using (var transacionScope = new TransactionScope())
            {
                if (!isUpdate)
                {
                    usuario = new Usuario();
                    var byteArrayPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.PASSWORD_DEFAULT,
                        ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                    usuario.Password = byteArrayPassword;
                }

                usuario.UserName = model.UserName;
                usuario.Nombres = model.Nombres;
                usuario.Apellidos = model.Apellidos;
                usuario.Documento = model.Documento;
                usuario.Telefono = model.Telefono;
                usuario.Correo = model.Correo;
                usuario.FechaNacimiento = model.FechaNacimiento;
                usuario.Estado = model.Estado;
                usuario.RolId = model.RolId;
                usuario.AreaId = model.AreaId;
                usuario.SapBusinessPartnerCardCode = model.SapBusinessPartnerCardCode;

                if (isUpdate)
                    DataContext.Context.Entry(usuario);
                else
                    DataContext.Context.Usuario.Add(usuario);
                DataContext.Context.SaveChanges();

                #region SubRoles

                //Desactiva todos los subroles de usuario
                foreach (var usuarioRol in DataContext.Context.UsuarioRoles.Where(x => x.UsuarioId == model.UsuarioId))
                {
                    usuarioRol.Estado = false;
                }

                //Activa todos los subroles de usuario que estén contenidos en model.RolList
                foreach (var rol in model.RolList)
                {
                    var usuarioRol = DataContext.Context.UsuarioRoles.FirstOrDefault(x => x.Rol.Codigo == rol.Codigo && x.UsuarioId == model.UsuarioId);
                    if (usuarioRol == null)
                    {
                        usuarioRol = new UsuarioRoles()
                        {
                            UsuarioId = usuario.UsuarioId,
                            RolId = rol.RolId,
                            Estado = true,
                        };

                        DataContext.Context.UsuarioRoles.Add(usuarioRol);
                    }
                    else
                    {
                        usuarioRol.Estado = true;
                        DataContext.Context.Entry(usuarioRol);
                    }
                }

                //Elimina todos los ids de niveles de aprobación:
                DataContext.Context.UsuarioNivelAprobacion.RemoveRange(
                    DataContext.Context.UsuarioNivelAprobacion.Where(x => x.UsuarioId == model.UsuarioId));

                //Inserta los permisos de aprobación de acuerdo a id's.
                foreach (var id in model.NivelAprobacionIdList)
                {
                    DataContext.Context.UsuarioNivelAprobacion.Add(
                        new UsuarioNivelAprobacion() { UsuarioId = usuario.UsuarioId, NivelAprobacionId = id });
                }

                DataContext.Context.SaveChanges();

                #endregion

                transacionScope.Complete();
            }
            return;
        }

        public void ChangePassword(ChangePasswordViewModel model)
        {
            var usuarioId = DataContext.Session.GetIdUsuario();
            var usuario = DataContext.Context.Usuario.FirstOrDefault(x => x.UsuarioId == usuarioId);

            if (usuario == null) return;
            usuario.Password = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
            DataContext.Context.SaveChanges();
        }

        public void DisableUsuario(int? UsuarioId)
        {
            var usuario = DataContext.Context.Usuario.Find(UsuarioId);
            if (usuario == null) return;

            usuario.Estado = !usuario.Estado;
            DataContext.Context.Entry(usuario);
            DataContext.Context.SaveChanges();
        }
    }
}
