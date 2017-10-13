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
    public class SAPMonedaLogic
    {
        public List<SAPMoneda> GetListSAPMonedas(DataContext dataContext)
        {
            return new SAPMonedaDataAccess().GetListMonedas(dataContext);
        }

        public SAPMoneda GetSAPMonedas(DataContext dataContext, Int32? idSAPMoneda)
        {
            return new SAPMonedaDataAccess().GetSAPMoneda(dataContext, idSAPMoneda);
        }

        public List<JsonEntity> GetJsonListSAPMonedas(DataContext dataContext)
        {
            return this.GetListSAPMonedas(dataContext).Select(x => new JsonEntity()
            {
                id = x.idSAPMoneda,
                text = x.DocCurrCod + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CurrName,
            }).ToList();
        }
    }
}
