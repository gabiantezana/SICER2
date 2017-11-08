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
        private DataContext DataContext { get; set; }
        public RolDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<Rol> GetList(RolNivel nivel)
        {
            return DataContext.Context.Rol.Where(x=> x.nivel == (int)nivel).ToList();
        }

        public Rol GetRol(int? rolId)
        {
            return DataContext.Context.Rol.FirstOrDefault(x => x.RolId == rolId);
        }

        public int AddUpdateRol(RolViewModel model)
        {
            Rol rol = DataContext.Context.Rol.Find(model.RolId);
            bool isUpdate = rol != null;

            if (!isUpdate)
                rol = new Rol();
            rol.Codigo = model.Codigo;
            rol.Descripcion = model.Descripcion;

            if (isUpdate)
                DataContext.Context.Entry(rol);
            else
                DataContext.Context.Rol.Add(rol);
            DataContext.Context.SaveChanges();
            return rol.RolId;
        }
    }
}
