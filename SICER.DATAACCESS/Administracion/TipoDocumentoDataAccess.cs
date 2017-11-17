using System.Collections.Generic;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;

namespace SICER.DATAACCESS.Administracion
{
    public class TipoDocumentoDataAccess
    {
        private DataContext DataContext { get; set; }

        public TipoDocumentoDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<TipoDocumento> GetList()
        {
            return DataContext.Context.TipoDocumento;
        }
    }
}
