using SICER.Controllers;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.VIEWMODEL.Administracion.Concepto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICER.Areas.Administracion.Controllers
{
    public class ConceptoController : BaseController
    {
        public ActionResult ListConceptos()
        {
            ListConceptosViewModel model = new ConceptoLogic().ListConceptosViewModel(GetDataContext());
            return View(model);
        }

        public ActionResult AddUpdateConcepto(Int32? idConcepto)
        {
            var model = new ConceptoLogic().GetConcepto(GetDataContext(), idConcepto);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUpdateConcepto(ConceptoViewModel model)
        {
            try
            {
                new ConceptoLogic().AddUpdateConcepto(GetDataContext(), model);
                PostMessage(MessageType.Success);
                return RedirectToAction(nameof(ListConceptos));
            }
            catch (CustomException ex)
            {
                PostMessage(ex);
                return View(model);
            }
        }
    }
}