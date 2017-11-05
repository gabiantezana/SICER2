using SICER.VIEWMODEL.Administracion.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.General;

namespace SICER.DATAACCESS.Administracion
{
    public class UsuarioDataAccess
    {
        private DataContext DataContext { get; set; }
        public UsuarioDataAccess(DataContext dataContext) { DataContext = dataContext; }

        public IEnumerable<Usuario> GetList()
        {
            return DataContext.Context.Usuario.ToList();
        }

        public Usuario GetUsuario(int? usuarioId)
        {
            return DataContext.Context.Usuario.Find(usuarioId);
        }

        public int AddUpdateUsuario(UsuarioViewModel model)
        {
            Usuario usuario = DataContext.Context.Usuario.Find(model.UsuarioId);
            bool isUpdate = usuario != null;

            if (!isUpdate)
            {
                usuario = new Usuario();
                var byteArrayPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.PASSWORD_DEFAULT, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
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
            usuario.SapBusinessPartnerCardCode = model.SapBusinessPartnerCardCode;

            if (isUpdate)
                DataContext.Context.Entry(usuario);
            else
                DataContext.Context.Usuario.Add(usuario);

            DataContext.Context.SaveChanges();
            return usuario.UsuarioId;
        }

        public void ChangePassword(ChangePasswordViewModel model)
        {
            var usuarioId = DataContext.Session.GetIdUsuario();
            var usuario = DataContext.Context.Usuario.FirstOrDefault(x => x.UsuarioId == usuarioId);

            if (usuario == null) return;
            usuario.Password = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
            DataContext.Context.SaveChanges();
        }
    }
}
