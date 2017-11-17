using System.Collections.Generic;
using PagedList;
using SICER.HELPER;

namespace SICER.VIEWMODEL.Administracion.NivelAprobacion
{
    public class ListNivelAprobacionViewModel
    {
        public IPagedList<MODEL.NivelAprobacion> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public List<MODEL.NivelAprobacion> ListDefault { get; set; }

    }
}
