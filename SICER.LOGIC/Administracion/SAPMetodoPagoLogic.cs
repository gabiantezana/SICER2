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
    public class SAPMetodoPagoLogic
    {
        public List<SAPMetodoPago> GetListSAPMetodoPago(DataContext dataContext)
        {
            return new SAPMetodoPagoDataAccess().GetListSAPMetodoPago(dataContext);
        }

        public SAPMetodoPago GetSAPMetodoPago(DataContext dataContext, Int32? idSAPMetodoPago)
        {
            return new SAPMetodoPagoDataAccess().GetSAPMetodoPago(dataContext, idSAPMetodoPago);
        }

        public List<JsonEntity> GetJsonListSAPMetodoPago(DataContext dataContext)
        {
            return this.GetListSAPMetodoPago(dataContext).Select(x => new JsonEntity()
            {
                id = x.idSAPMetodoPago,
                text = x.PayMethCod + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.Descript,
            }).ToList();
        }
    }
}
