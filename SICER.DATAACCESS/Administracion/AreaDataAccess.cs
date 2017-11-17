using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Area;

namespace SICER.DATAACCESS.Administracion
{
    public class AreaDataAccess
    {
        private DataContext DataContext { get; set; }
        public AreaDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<Area> GetList(string filter)
        {
            var query = DataContext.Context.Area.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
               query =  filter.ToLower().Split(' ').Aggregate( query, (current, token) => current.Where(x=> x.Descripcion.ToLower().Contains(token)));
            }
            return query;
        }

        public Area GetEntity(int? AreaId)
        {
            return DataContext.Context.Area.Find(AreaId);
        }

        public void AddUpdate(AreaViewModel model)
        {
            var area = DataContext.Context.Area.Find(model.AreaId);
            var isUpdate = area != null;

            if (!isUpdate)
            {
                area = new Area();
            }
            area.Descripcion = model.Descripcion;
            if (isUpdate)
                DataContext.Context.Entry(area);
            else
                DataContext.Context.Area.Add(area);
            DataContext.Context.SaveChanges();

        }
    }
    
}
