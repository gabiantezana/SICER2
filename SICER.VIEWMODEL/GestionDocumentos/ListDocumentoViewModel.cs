using System.Collections.Generic;
using PagedList;
using SICER.HELPER;

namespace SICER.VIEWMODEL.GestionDocumentos
{
    public class ListDocumentoViewModel
    {
        //public IPagedList<MODEL.Documento> PagedList { get; set; }
        public IPagedList<DocumentoViewModel> PagedList { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public IEnumerable<MODEL.Documento> ListDefault { get; set; }
        public DocumentType DocumentType { get; set; }

    }
}
