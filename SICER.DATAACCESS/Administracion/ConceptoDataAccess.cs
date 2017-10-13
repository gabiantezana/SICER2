using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class ConceptoDataAccess
    {
        public List<Concepto> GetListConcepto(DataContext dataContext)
        {
            return dataContext.Context.Concepto.ToList();
        }

        public Concepto GetConcepto(DataContext dataContext, Int32? idConcepto)
        {
            return dataContext.Context.Concepto.FirstOrDefault(x => x.idConcepto == idConcepto);
        }
    }
}
