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


        public static List<RolViewModel> GetList(DataContext dataContext, RolNivel nivel)
        {
            var modelList = new List<RolViewModel>();

            IEnumerable<Rol> list = new RolDataAccess(dataContext).GetList(nivel);
            list.ToList().ForEach(x => modelList.Add(GetRolViewModel(x)));

            return modelList;
        }

        public static List<JsonEntity> GetJList(DataContext dataContext, RolNivel nivel)
        {
            return new RolDataAccess(dataContext).GetList(nivel).Where(x=> x.nivel == (int)nivel).Select(x => new JsonEntity()
            {
                id = x.RolId,
                text = x.Descripcion
            }).ToList();
        }

        public static RolViewModel GetRol(DataContext dataContext, int? rolId)
        {
            var rol = new RolDataAccess(dataContext).GetRol(rolId);

            RolViewModel model = rol.ConvertTo(typeof(RolViewModel));
            FillJLists(dataContext, ref model);
            return model;
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
            var model = new    RolViewModel();
            if (rol == null)
                return model;

            model = rol.ConvertTo(typeof(RolViewModel));
            return model;
        }

        private static List<JsonEntity> GetNivelJList()
        {
            return new List<JsonEntity>()
            {
                new JsonEntity()
                {
                    id = 1,
                    text = "Primario",
                },
                new JsonEntity()
                {
                    id = 2,
                    text = "Secundario",
                }
            };
        }

        private static void FillJLists(DataContext dataContext, ref RolViewModel model)
        {
            model.NivelJList = GetNivelJList();
        }
    }
}
