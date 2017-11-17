using System.Collections.Generic;
using PagedList;

namespace SICER.VIEWMODEL.Administracion.TipoDocumento
{
   public class ListTipoDocumento
    {
        public IPagedList<MODEL.TipoDocumento> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public List<MODEL.TipoDocumento> ListDefault { get; set; }
    }
}
