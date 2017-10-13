using SICER.Controllers;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICER.Areas.Administracion.Controllers
{
    public class DocumentoController : BaseController
    {
        #region Get

        public ActionResult ListDocumentos(DocumentType documentType)
        {
            var model = new DocumentoLogic().ListDocumentos(GetDataContext(), documentType);
            ViewBag.DocumentType = documentType;
            return View(model);
        }

        public ActionResult AddUpdateDocumento(DocumentType documentType, Int32? idDocumento)

        {
            var model = new DocumentoLogic().GetDocumento(GetDataContext(), idDocumento);
            model.DocumentType = documentType;
            return View(model);
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult AddUpdateDocumento(DocumentoViewModel model)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    Int32? idDocumento = new DocumentoLogic().AddUpdateDocumento(GetDataContext(), model);
                    return RedirectToAction(nameof(AddUpdateDocumento), idDocumento);
                }
                catch (CustomException ex)
                {
                    PostMessage(ex);
                }
            }
            return View(model);
        }



        public ActionResult RefreshDetalleDocumento(List<DocumentoDetalleViewModel> listDetalleDocumento)
        {
            var model = new List<DocumentoDetalle>();
            return PartialView("PartialView/_ListDetalleDocumentos", model);
        }


        #endregion

        #region Modals

        public ActionResult ModalDocumentoDetalle(DocumentoDetalleViewModel model)
        {
            return View("Modal/ModalDocumentoDetalle", new DocumentoDetalleViewModel());
            if (model?.IdDocumentoDetalle == null)
            {
                model = new DocumentoLogic().DetalleDocumentoViewModel(GetDataContext());
            }

            new DocumentoLogic().FillLists(GetDataContext(), ref model);

            return View("Modal/ModalDocumentoDetalle", model);
        }

        [HttpPost]
        public JsonResult ModalDocumentoDetallePost(DocumentoDetalleViewModel model)
        {
            return Json(true);
        }

        #endregion

        #region PartialView

        [HttpPost]
        public ActionResult _PartialDetalleDocumentos(DocumentoDetalleViewModel model, List<DocumentoDetalleViewModel> ListDetalle)
        {
            try
            {
                new DocumentoLogic().AddUpdateDocumentoDetalle(GetDataContext(), model,  ref ListDetalle);

                DocumentoViewModel modelToReturn = new DocumentoViewModel();
                modelToReturn.ListDetalle = ListDetalle;
                return PartialView("PartialView/_ListDetalleDocumentos", modelToReturn);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [HttpPost]
        public ActionResult _AddUpdateDetalleDocumento(DocumentoDetalleViewModel model)
        {
            /*if (ModelState.IsValid)
                return AjaxException(TypeAjaxException.Error, "Model is not valid.");

            else*/
            return PartialView("PartialView/_AddUpdateDetalleDocumento", model);

        }

        #endregion
    }


}
