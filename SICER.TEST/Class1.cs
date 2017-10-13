using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.TEST
{
    public class Class1
    {
        public Class1()
        {
            DataContext dataContext = new DataContext();
            dataContext.Context = new SICEREntities();
            new SICER.DATAACCESS.Administracion.SyncDataAccess().Sync_SAPProveedor(dataContext);
        }
    }
}
