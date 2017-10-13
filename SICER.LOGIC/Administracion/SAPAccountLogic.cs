using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.LOGIC.Administracion
{
    public class SAPAccountLogic
    {
        public List<SAPAccount> GetListSAPAccounts(DataContext dataContext)
        {
            return new SAPAccountDataAccess().ListSAPAccount(dataContext);
        }

        public SAPAccount GetSAPCentroCostos(DataContext dataContext, Int32? idSAPAccount)
        {
            return new SAPAccountDataAccess().GetSAPAccount(dataContext, idSAPAccount);
        }

        public List<JsonEntity> GetJsonListSAPAccounts(DataContext dataContext)
        {
            return this.GetListSAPAccounts(dataContext).Select(x => new JsonEntity()
            {
                id = x.idSAPAccount,
                text = x.AcctCode + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.AcctName,
            }).ToList();
        }
    }
}
