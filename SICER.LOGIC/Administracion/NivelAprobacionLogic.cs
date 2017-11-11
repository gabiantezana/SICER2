using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.DATAACCESS.Administracion;
using SICER.MODEL;

namespace SICER.LOGIC.Administracion
{
    public class NivelAprobacionLogic
    {
        public IQueryable GetList(DataContext dataContext)
        {
            return new NivelAprobacionDataAccess(dataContext).GetList();
        }

        public NivelAprobacion GetNivelAprobacion(DataContext dataContext, int? nivelAprobacionId)
        {
            return new NivelAprobacionDataAccess(dataContext).GetNivelAprobacion((nivelAprobacionId));
        }
    }
}
