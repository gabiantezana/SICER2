using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Rol;

namespace SICER.LOGIC.Administracion
{
    public class RolLogic
    {
        public static List<RolViewModel> GetList(DataContext dataContext)
        {
            var modelList = new List<RolViewModel>();

            IEnumerable<Rol> list = new RolDataAccess(dataContext).GetList();
            list.ToList().ForEach(x => modelList.Add(GetRolViewModel(x)));

            return modelList;
        }

        public static List<JsonEntity> GetJsonList(DataContext dataContext)
        {
            return new RolDataAccess(dataContext).GetList().Select(x => new JsonEntity()
            {
                id = x.RolId,
                text = x.Codigo
            }).ToList();
        }

        public static RolViewModel GetRol(DataContext dataContext, int? rolId)
        {
            Rol rol = new RolDataAccess(dataContext).GetRol(rolId);
            return rol.ConvertTo(typeof(RolViewModel));
        }

        public static int AddUpdateRol(DataContext dataContext, RolViewModel model)
        {
            return new RolDataAccess(dataContext).AddUpdateRol(model);
        }

        //TODO:
        public static LstRolUsuarioViewModel EditPermisosPorRoles(DataContext dataContext, int rolId)
        {
            var model = new LstRolUsuarioViewModel
            {
                RolId = rolId,
                LstGrupoVistas = dataContext.Context.GrupoVista.ToList(),
                LstVistas = dataContext.Context.Vista.ToList(),
                LstVistasRol = dataContext.Context.VistaRol.Where(x => x.RolId == rolId).ToList()
            };
            return model;
        }

        private static RolViewModel GetRolViewModel(Rol rol)
        {
            return rol == null ? new RolViewModel() : rol.ConvertTo(typeof(RolViewModel));
        }
    }
}
