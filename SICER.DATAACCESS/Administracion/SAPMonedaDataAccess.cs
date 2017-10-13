using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SAPMonedaDataAccess
    {
        public List<SAPMoneda> GetListMonedas(DataContext dataContext)
        {
            return dataContext.Context.SAPMoneda.ToList();
        }

        public SAPMoneda GetSAPMoneda(DataContext dataContext, Int32? idMoneda)
        {
            return dataContext.Context.SAPMoneda.Find(idMoneda);
        }
    }
}
