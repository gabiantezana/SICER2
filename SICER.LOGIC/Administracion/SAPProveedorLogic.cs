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
    public class SAPProveedorLogic
    {
        public List<SAPProveedor> GetListSAPProveedors(DataContext dataContext)
        {
            return new SAPProveedorDataAccess().GetListSAPProveedor(dataContext);
        }

        public SAPProveedor GetSAPProveedors(DataContext dataContext, Int32? idSAPProveedor)
        {
            return new SAPProveedorDataAccess().GetSAPProveedor(dataContext, idSAPProveedor);
        }

        public List<JsonEntity> GetJSONListSAPProveedors(DataContext dataContext)
        {
            return this.GetListSAPProveedors(dataContext).Select(x => new JsonEntity()
            {
                id = x.idSAPProveedor,
                text = x.LictradNum + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CardName,
            }).ToList();
        }
    }
}
