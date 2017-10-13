using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SAPAccountDataAccess
    {
        public List<SAPAccount> ListSAPAccount(DataContext dataContext)
        {
            return dataContext.Context.SAPAccount.ToList();
        }

        public SAPAccount GetSAPAccount(DataContext dataContext, Int32? idSAPAccount)
        {
            return dataContext.Context.SAPAccount.Find(idSAPAccount);
        }
    }
}
