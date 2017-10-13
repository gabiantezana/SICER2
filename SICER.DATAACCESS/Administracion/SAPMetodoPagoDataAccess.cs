using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SAPMetodoPagoDataAccess
    {
        public List<SAPMetodoPago> GetListSAPMetodoPago(DataContext dataContext)
        {
            return dataContext.Context.SAPMetodoPago.ToList();
        }

        public SAPMetodoPago GetSAPMetodoPago(DataContext dataContext, Int32? idSAPMetodoPago)
        {
            return dataContext.Context.SAPMetodoPago.Find(idSAPMetodoPago);
        }
    }
}
