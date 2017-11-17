using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.HELPER;

namespace SICER.VIEWMODEL.Administracion.Area
{
    public class ListAreaViewModel
    {
        public IPagedList<MODEL.Area> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>|
        public IEnumerable<MODEL.Documento> ListDefault { get; set; }
    }
}
