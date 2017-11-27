using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SICER.Controllers;
using SICER.EXCEPTION;
using SICER.Filters;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.LOGIC.GestionDocumentos;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.NivelAprobacion;
using SICER.VIEWMODEL.GestionDocumentos;

namespace SICER.Areas.GestionDocumentos.Controllers
{
    public class DocumentoController : BaseController
    {
        #region Get

        public ActionResult List(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.CajaChica:
                    return RedirectToAction(nameof(ListCC));
                case DocumentType.EntregaARendir:
                    return RedirectToAction(nameof(ListER));
                case DocumentType.Reembolso:
                    return RedirectToAction(nameof(ListRE));
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null);
            }
        }

        public ActionResult ListCC()
        {
            var model = DocumentoLogic.GetListDocumentoViewModel(GetDataContext(), DocumentType.CajaChica, null, null);
            ViewBag.DocumentType = DocumentType.CajaChica;
            return View("List", model);
        }

        public ActionResult ListER()
        {
            var model = DocumentoLogic.GetListDocumentoViewModel(GetDataContext(), DocumentType.EntregaARendir, null, null);
            ViewBag.DocumentType = DocumentType.EntregaARendir;
            return View("List", model);
        }

        public ActionResult ListRE()
        {
            var model = DocumentoLogic.GetListDocumentoViewModel(GetDataContext(), DocumentType.Reembolso, null, null);
            ViewBag.DocumentType = DocumentType.Reembolso;
            return View("List", model);
        }


        public ActionResult AddUpdate(DocumentType documentType, int? documentoId)
        {
            switch (documentType)
            {
                case DocumentType.CajaChica:
                    return RedirectToAction(nameof(AddUpdateCC), new { documentoId = documentoId });
                case DocumentType.EntregaARendir:
                    return RedirectToAction(nameof(AddUpdateER), new { documentoId = documentoId });
                case DocumentType.Reembolso:
                    return RedirectToAction(nameof(AddUpdateRE), new { documentoId = documentoId });
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null);
            }
        }

        public ActionResult AddUpdateCC(int? documentoId)
        {
            ModelState.Remove("Serie");
            var model = DocumentoLogic.GetOpening(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            model.TipoDocumentoId = (int) DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        public ActionResult AddUpdateER(int? documentoId)
        {
            var model = DocumentoLogic.GetOpening(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        public ActionResult AddUpdateRE(int? documentoId)
        {
            var model = DocumentoLogic.GetOpening(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult AddUpdate(DocumentoViewModel model)
        {
            ModelState.Remove("Serie");
            ModelState.Remove("Correlativo");
            ModelState.Remove("SapConceptoCode");
            if (ModelState.IsValid)
            {
                try
                {
                    DocumentoLogic.AddUpdateDocument(GetDataContext(), model, model.SubmitType);
                    PostMessage(MessageType.Success);
                    return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
                }
                catch (CustomException ex)
                {
                    PostMessage(ex);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                DocumentoLogic.FillJLists(GetDataContext(), ref model);
            }
            return View(model);
        }

        public ActionResult RefreshDetalleDocumento(List<DocumentoViewModel> listDetalleDocumento)
        {
            var model = new List<DocumentoViewModel>();
            return PartialView("PartialView/_ListDetalleDocumentos", model);
        }

        [HttpPost]
        public ActionResult ApproveApertura(DocumentoViewModel model)
        {
            DocumentoLogic.ApproveDocument(GetDataContext(), model);
            PostMessage(MessageType.Success, "Documento aprobado exitosamente.");
            return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
        }

        [HttpPost]
        public ActionResult RechazarDocumento(DocumentoViewModel model)
        {
            DocumentoLogic.RefuseDocument(GetDataContext(), model.DocumentoId, model.MotivoRechazo);
            PostMessage(MessageType.Success, "Documento rechazado exitosamente.");
            return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
        }

        #endregion

        #region Modals

        public ActionResult ModalRendicion(int? aperturaDocumentoId, int? documentoId)
        {
            //ModelState.Clear();
            var model = DocumentoLogic.GetExpenditure(GetDataContext(), aperturaDocumentoId, documentoId);
            DocumentoLogic.FillJLists(GetDataContext(), ref model);

            return View("Modal/ModalRendicion", model);
        }

        [HttpPost]
        public JsonResult ModalRendicionPost(DocumentoViewModel model)
        {
            return Json(true);
        }

        #endregion

        #region PartialView

        /// <summary>
        /// Recibe una rendición y el listado actual de rendiciones. Agrega o modifica la rendición enviada y vuelve a renderizar el listado.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _RendicionListCopy(DocumentoViewModel model, List<DocumentoViewModel> currentList)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        //TODO: VALIDATE 
            //        DocumentoLogic.AddUpdateRendicion(GetDataContext(), model);

            //        currentList = currentList ?? new List<DocumentoViewModel>();
            //        currentList.Add(model);

            //        DocumentoViewModel modelToReturn = new DocumentoViewModel { RendicionList = currentList };
            //        return PartialView("PartialView/_RendicionList", modelToReturn);
            //    }
            //    catch (Exception ex)
            //    {
            //        return AjaxException(TypeAjaxException.Error, ex);
            //    }
            //}
            //else //TODO:
            return AjaxException(TypeAjaxException.Error, "Modelo no válido");
        }

        [HttpPost]
        public ActionResult _RendicionList(DocumentoViewModel model)
        {
            try
            {
                //TODO: VALIDATE 
                //Agrega o modifica rendición
                DocumentoLogic.AddUpdateDocument(GetDataContext(), model, model.SubmitType);

                //Devuelve nuevo listado
                var list = DocumentoLogic.GetExpenditureList(GetDataContext(), model.AperturaDocumentoId, null);

                return PartialView("PartialView/_RendicionList", list);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [HttpPost]
        public ActionResult _AddUpdateDetalleDocumento(DocumentoViewModel model)
        {
            /*if (ModelState.IsValid)
                return AjaxException(TypeAjaxException.Error, "Model is not valid.");

            else*/
            return PartialView("PartialView/_AddUpdateDetalleDocumento", model);

        }

        /// <summary>
        /// Lista paginada
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult _PagedList(ListDocumentoViewModel model, int? page)
        {
            ViewBag.filter = model.Filter;
            ViewBag.DocumentType = model.DocumentType;
            model.PagedList = DocumentoLogic.GetOpeningPagedList(GetDataContext(), model.DocumentType, model.Filter, page);
            return PartialView("PartialView/_PagedList", model.PagedList);
        }

        /// <summary>
        /// Lista filtrada
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult _FilterList(string filter)
        {
            ViewBag.filter = filter;
            // var list = NivelAprobacionLogic.GetPagedList(GetDataContext(), filter, null);
            return PartialView("PartialView/_PagedList", null);
        }

        #endregion
    }


}
