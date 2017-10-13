using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SICER.HELPER;
using SICER.VIEWMODEL.General;

namespace SICER.DATAACCESS.Administracion
{
    public class UsuarioDataAccess
    {
        public List<UsuarioViewModel> LstUsuarios(DataContext contextData)
        {
            List<UsuarioViewModel> lstUsuarios = new List<UsuarioViewModel>();
            List<Usuario> lstUsuariosDB = contextData.Context.Usuario.Include(x => x.Rol).ToList();
            foreach (var usuario in lstUsuariosDB)
            {
                lstUsuarios.Add(new UsuarioViewModel
                {
                    IdUsuario = usuario.idUsuario,
                    Correo = usuario.correo,
                    Documento = usuario.documento,
                    IdRol = usuario.idRol ?? 1,
                    Telefono = usuario.telefono,
                    Estado = usuario.estado ?? false,
                    Nombres = usuario.nombres,
                    Usuario = usuario.username,
                });
            }
            return lstUsuarios;
        }

        public UsuarioViewModel GetUsuario(DataContext context, Int32? idUsuario)
        {
            Usuario usuarioDB = context.Context.Usuario.FirstOrDefault(x => x.idUsuario == idUsuario);
            UsuarioViewModel viewModel = new UsuarioViewModel();

            if (usuarioDB != null)
            {
                viewModel.Correo = usuarioDB.correo;
                viewModel.Documento = usuarioDB.documento;
                viewModel.IdRol = usuarioDB.idRol ?? 0;
                viewModel.Estado = usuarioDB.estado ?? false;
                viewModel.IdUsuario = usuarioDB.idUsuario;
                viewModel.Nombres = usuarioDB.nombres;
                viewModel.Telefono = usuarioDB.telefono;
                viewModel.Usuario = usuarioDB.username;
            }
            FillLists(ref viewModel, context);
            return viewModel;
        }

        public void FillLists(ref UsuarioViewModel model, DataContext contextData)
        {
            model.LstRol = new RolDataAccess().LstRolJsonEntity(contextData);
        }

        public Int32 AddUpdateUsuario(DataContext dataContext, UsuarioViewModel model)
        {
            Usuario usuario = dataContext.Context.Usuario.FirstOrDefault(x => x.idUsuario == model.IdUsuario);
            Boolean esUpdate = usuario == null ? false : true;

            if (!esUpdate)
            {
                usuario = new Usuario();
                Byte[] byteArrayPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.DEFAULT_PASSWORD, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                usuario.password = byteArrayPassword;
            }

            usuario.nombres = model.Nombres;
            usuario.username = model.Usuario;
            usuario.telefono = model.Telefono;
            usuario.correo = model.Correo;
            usuario.estado = model.Estado;
            usuario.documento = model.Documento;
            usuario.idRol = model.IdRol;

            if (esUpdate)
                dataContext.Context.Entry(usuario);
            else
                dataContext.Context.Usuario.Add(usuario);

            dataContext.Context.SaveChanges();
            return usuario.idUsuario;
        }

        public void ChangePassword(DataContext DataContext, ChangePasswordViewModel model)
        {
            try
            {
                var usuarioId = DataContext.Session.GetIdUsuario();
                Usuario usuario = DataContext.Context.Usuario.FirstOrDefault(x => x.idUsuario == usuarioId);
                usuario.password = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                DataContext.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
