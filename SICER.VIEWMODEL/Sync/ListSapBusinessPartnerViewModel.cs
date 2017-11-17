using System.Collections.Generic;
using PagedList;

namespace SICER.VIEWMODEL.Sync
{
   public class ListSapBusinessPartnerViewModel
    {
        public IPagedList<MODEL.SapBusinessPartner> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public IEnumerable<MODEL.SapBusinessPartner> ListDefault { get; set; }
    }
}
