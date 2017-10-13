using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SAPCentroCostosDataAccess
    {
        public List<SAPCentroCostos> GetListSAPCentrosCost(DataContext dataContext)
        {
            return dataContext.Context.SAPCentroCostos.ToList();
        }

        public SAPCentroCostos GetSAPCentroCosto(DataContext dataContext, Int32? idSAPCentroCosto)
        {
            return dataContext.Context.SAPCentroCostos.Find(idSAPCentroCosto);
        }
    }
}
