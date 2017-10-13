using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Concepto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.LOGIC.Administracion
{
    public class ConceptoLogic
    {
        private List<ConceptoViewModel> _ListConceptos(DataContext dataContext)
        {
            return new ConceptoDataAccess().GetListConcepto(dataContext).Select(x => new ConceptoViewModel()
            {
                Descripcion = x.descripcion,
                IdAccount = x.idAccount,
                IdConcepto = x.idConcepto,
                Nombre = x.nombre,
            }).ToList();
        }

        public IPagedList<ConceptoViewModel> ListConceptos(DataContext dataContext)
        {
            return _ListConceptos(dataContext).ToPagedList(1, 500);
        }

        public ListConceptosViewModel ListConceptosViewModel(DataContext dataContext)
        {
            ListConceptosViewModel model = new ListConceptosViewModel();
            model.ListConceptos = ListConceptos(dataContext);
            return model;
        }

        public ConceptoViewModel GetConcepto(DataContext dataContext, Int32? idConcepto)
        {
            ConceptoViewModel model = new ConceptoViewModel();
            Concepto concepto = new ConceptoDataAccess().GetConcepto(dataContext, idConcepto);
            if (concepto != null)
            {
                model.Descripcion = concepto.descripcion;
                model.IdAccount = concepto.idAccount;
                model.IdConcepto = concepto.idConcepto;
                model.Nombre = concepto.nombre;
            }

            FillLists(dataContext, ref model);
            return model;
        }

        public Int32? AddUpdateConcepto(DataContext dataContext, ConceptoViewModel model)
        {
            Boolean esUpdate = true;
            Concepto concepto = dataContext.Context.Concepto.Find(model.IdConcepto);
            if (concepto == null)
                esUpdate = false;

            if (!esUpdate)
                concepto = new Concepto();

            concepto.nombre = model.Nombre;
            concepto.descripcion = model.Descripcion;
            concepto.idAccount = model.IdAccount;


            if (!esUpdate)
                dataContext.Context.Concepto.Add(concepto);
            else
                dataContext.Context.Entry(concepto);

            dataContext.Context.SaveChanges();
            return concepto.idConcepto;
        }

        public void FillLists(DataContext dataContext, ref ConceptoViewModel model)
        {
            model.ListSAPAccounts = new SAPAccountLogic().GetJsonListSAPAccounts(dataContext);
        }

        public List<JsonEntity> JsonListConceptos(DataContext dataContext)
        {
            return new ConceptoDataAccess().GetListConcepto(dataContext).Select(x => new JsonEntity()
            {
                id = x.idConcepto,
                text = x.nombre,
            }).ToList();
        }
    }
}
