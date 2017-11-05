using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.DATAACCESS.Administracion;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.VIEWMODEL.Administracion.Usuario;
using SICER.VIEWMODEL.General;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Rol;

namespace SICER.LOGIC.Administracion
{
    public class UsuarioLogic
    {
        private UsuarioLogic() { }

        public static void FillJLists(DataContext dataContext, ref UsuarioViewModel model)
        {
            model.RolJList = RolLogic.GetJsonList(dataContext);
        }

        public static IEnumerable<Usuario> GetList(DataContext dataContext)
        {
            return new UsuarioDataAccess(dataContext).GetList();
        }

        /*public List<UsuarioViewModel> GetList(DataContext dataContext)
        {
            var usuarioViewModelList = new List<UsuarioViewModel>();

            IEnumerable<Usuario> usuarioList = new UsuarioDataAccess().GetList(dataContext);
            usuarioList.ToList().ForEach(x=> usuarioViewModelList.Add(GetUsuarioViewModel(x)));

            return usuarioViewModelList;
        }*/

        public static UsuarioViewModel GetUsuario(DataContext dataContext, int? usuarioId)
        {
            Usuario usuario = new UsuarioDataAccess(dataContext).GetUsuario(usuarioId);
            return GetUsuarioViewModel(dataContext, usuario);
        }

        public static int AddUpdateUsuario(DataContext dataContext, UsuarioViewModel model)
        {
            ValidarDuplicado(dataContext, model);
            return new UsuarioDataAccess(dataContext).AddUpdateUsuario(model);
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

        private  static void ValidarDuplicado(DataContext dataContext, UsuarioViewModel model)
        {
            const string message = "El campo UserName* ya existe para otro usuario.";
            var usuario = dataContext.Context.Usuario.FirstOrDefault(x => x.UserName == model.UserName
                                                                         && x.UsuarioId != model.UsuarioId
                                                                         );
            if(usuario != null)
                throw new CustomException(new TempDataEntityException { Mensaje = message, TipoMensaje = MessageTypeException.Warning }, dataContext);
        }

    }
}

