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
    public class SAPCentroCostosLogic
    {
        public List<SAPCentroCostos> GetListSAPCentroCostos(DataContext dataContext)
        {
            return new SAPCentroCostosDataAccess().GetListSAPCentrosCost(dataContext);
        }

        public SAPCentroCostos GetSAPCentroCostos(DataContext dataContext, Int32? idSAPCentroCostos)
        {
            return new SAPCentroCostosDataAccess().GetSAPCentroCosto(dataContext, idSAPCentroCostos);
        }

        public List<JsonEntity> GetJsonListSAPCentroCostos(DataContext dataContext)
        {
            return this.GetListSAPCentroCostos(dataContext).Select(x => new JsonEntity()
            {
                id = x.idSAPCentroCostos,
                text = x.PrcCode + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.PrcName,
            }).ToList();
        }


    }
}
