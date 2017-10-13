using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SAPProveedorDataAccess
    {
        public List<SAPProveedor> GetListSAPProveedor(DataContext dataContext)
        {
            return dataContext.Context.SAPProveedor.ToList();
        }

        public SAPProveedor GetSAPProveedor(DataContext dataContext, Int32? idProveedor)
        {
            return dataContext.Context.SAPProveedor.Find(idProveedor);
        }
    }
}
