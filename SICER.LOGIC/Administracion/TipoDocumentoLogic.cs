using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;

namespace SICER.LOGIC.Administracion
{
    public static class TipoDocumentoLogic
    {
        private static IEnumerable<TipoDocumento> GetQueryList(DataContext dataContext, string filtro = null)
        {
            return new TipoDocumentoDataAccess(dataContext).GetList();
        }

        public static IEnumerable<JsonEntity> GetJList(DataContext dataContext)
        {
            var query = GetQueryList(dataContext);
            var jsonEntities = query?.Select(x => new JsonEntity()
            {
                id = x.TipoDocumentoId,
                text = x.Descripcion
            });

            return jsonEntities;
        }
    }
}
