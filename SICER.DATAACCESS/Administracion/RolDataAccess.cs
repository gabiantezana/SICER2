using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class RolDataAccess
    {
        public List<Rol> LstRoles(DataContext contextData)
        {
            return contextData.Context.Rol.ToList();
        }

        public List<JsonEntity> LstRolJsonEntity(DataContext context)
        {

            return LstRoles(context).Select(x => new JsonEntity()
            {
                id = x.idRol,
                text = x.nombre
            }).ToList() ?? new List<JsonEntity>();
        }

        public RolViewModel GetRol(DataContext DataContext, Int32? idRol)
        {
            RolViewModel model = new RolViewModel();

            var rol = DataContext.Context.Rol.FirstOrDefault(x => x.idRol == idRol);

            if (rol != null)
            {
                model.Codigo = rol.nombre;
                model.Nombre = rol.descripcion;
                model.idRol = rol.idRol;
            }


            return model;
        }

        public LstRolUsuarioViewModel EditPermisosPorRoles(DataContext DataContext, Int32 idRol)
        {
            LstRolUsuarioViewModel model = new LstRolUsuarioViewModel();
            model.idRol = idRol;
            model.LstGrupoVistas = DataContext.Context.VistasGrupo.ToList();
            model.LstVistas = DataContext.Context.Vistas.Where(x => x.idVistasGrupo != null).ToList();
            model.LstVistasRol = DataContext.Context.VistasRol.Where(x => x.idRol == idRol).ToList();

            return model;

        }
    }
}
