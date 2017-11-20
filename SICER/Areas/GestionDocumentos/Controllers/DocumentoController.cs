﻿using System;
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

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.CajaChica.LISTAR)]
        public ActionResult ListCC()
        {
            var model = DocumentoLogic.GetListDocumentoViewModel(GetDataContext(), DocumentType.CajaChica, null, null);
            ViewBag.DocumentType = DocumentType.CajaChica;
            return View("List", model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.EntregaRendir.LISTAR)]
        public ActionResult ListER()
        {
            var model = DocumentoLogic.GetListDocumentoViewModel(GetDataContext(), DocumentType.EntregaARendir, null, null);
            ViewBag.DocumentType = DocumentType.EntregaARendir;
            return View("List", model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.Reembolso.LISTAR)]
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

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.CajaChica.CREAR, ConstantHelper.Vistas.Documento.CajaChica.APROBAR)]
        public ActionResult AddUpdateCC(int? documentoId)
        {
            var model = DocumentoLogic.GetApertura(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.CajaChica.CREAR, ConstantHelper.Vistas.Documento.CajaChica.APROBAR)]
        public ActionResult AddUpdateER(int? documentoId)
        {
            var model = DocumentoLogic.GetApertura(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        [AppViewAuthorize(ConstantHelper.Vistas.Documento.CajaChica.CREAR, ConstantHelper.Vistas.Documento.CajaChica.APROBAR)]
        public ActionResult AddUpdateRE(int? documentoId)
        {
            var model = DocumentoLogic.GetApertura(GetDataContext(), documentoId);
            model.DocumentType = DocumentType.CajaChica;
            return View("AddUpdate", model);
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult AddUpdate(DocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DocumentoLogic.AddUpdateApertura(GetDataContext(), model);
                    PostMessage(MessageType.Success);
                    return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
                }
                catch (CustomException ex)
                {
                    PostMessage(ex);
                }
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
            DocumentoLogic.ApproveApertura(GetDataContext(), model);
            PostMessage(MessageType.Success, "Documento aprobado exitosamente.");
            return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
        }

        [HttpPost]
        public ActionResult RechazarDocumento(DocumentoViewModel model)
        {
            DocumentoLogic.RechazarApertura(GetDataContext(), model.DocumentoId, model.MotivoRechazo);
            PostMessage(MessageType.Success, "Documento rechazado exitosamente.");
            return RedirectToAction(nameof(List), new { documentType = model.DocumentType });
        }

        #endregion

        #region Modals

        public ActionResult ModalRendicion(int? aperturaDocumentoId, int? documentoId)
        {
            ModelState.Clear();

            var model = DocumentoLogic.GetRendicion(GetDataContext(), aperturaDocumentoId, documentoId);
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
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: VALIDATE 
                    //Agrega o modifica rendición
                    DocumentoLogic.AddUpdateRendicion(GetDataContext(), model);

                    //Devuelve nuevo listado
                    var list = DocumentoLogic.GetRendicionesList(GetDataContext(), model.AperturaDocumentoId, null);

                    return PartialView("PartialView/_RendicionList", list);
                }
                catch (Exception ex)
                {
                    return AjaxException(TypeAjaxException.Error, ex);
                }
            }
            else //TODO:
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                return AjaxException(TypeAjaxException.Error, "Error de validación");
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
            model.PagedList = DocumentoLogic.GetPagedList(GetDataContext(), model.DocumentType, model.Filter, page);
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
