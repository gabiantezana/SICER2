using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Concepto
{
    public class ListConceptosViewModel
    {
        public IPagedList<ConceptoViewModel> ListConceptos { get; set; }
    }
}
