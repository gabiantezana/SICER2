using System.Collections.Generic;
using PagedList;

namespace SICER.VIEWMODEL.Sync
{
   public class ListOstcViewModel
    {
        public IPagedList<MODEL.OSTC> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public IEnumerable<MODEL.OSTC> ListDefault { get; set; }
    }
}
