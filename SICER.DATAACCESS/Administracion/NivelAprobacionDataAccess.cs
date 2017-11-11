using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.DATAACCESS.Administracion
{
    public class NivelAprobacionDataAccess
    {
        private DataContext DataContext { get; set; }

        public NivelAprobacionDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IQueryable GetList()
        {
            return DataContext.Context.NivelAprobacion;
        }

        public NivelAprobacion GetNivelAprobacion(int? nivelAprobacionId)
        {
            return DataContext.Context.NivelAprobacion.Find(nivelAprobacionId);
        }
    }
}
