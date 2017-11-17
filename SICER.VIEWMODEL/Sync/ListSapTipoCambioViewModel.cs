using System.Collections.Generic;
using PagedList;

namespace SICER.VIEWMODEL.Sync
{
   public class ListSapTipoCambioViewModel
    {
        public IPagedList<MODEL.SapTipoCambio> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public List<MODEL.SapTipoCambio> ListDefault { get; set; }
    }
}
