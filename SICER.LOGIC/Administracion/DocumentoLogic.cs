using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.LOGIC.Administracion
{
    public class DocumentoLogic
    {
        public ListDocumentoViewModel ListDocumentos(DataContext dataContext, DocumentType documentType)
        {
            ListDocumentoViewModel model = new ListDocumentoViewModel();
            model.ListDocumentos = IPagedListDocumentos(dataContext, documentType);
            return model;
        }

        private List<Documento> GetDocumentos(DataContext dataContext, DocumentType documentType)
        {
            return new DocumentoDataAccess().GetListDocumento(dataContext, documentType);
        }

        private IPagedList<Documento> IPagedListDocumentos(DataContext dataContext, DocumentType documentType)
        {
            return GetDocumentos(dataContext, documentType).ToPagedList(1, 30);
        }

        public DocumentoViewModel GetDocumento(DataContext dataContext, Int32? idDocumento)
        {
            var documento = new DocumentoDataAccess().GetDocumento(dataContext, idDocumento);
            DocumentoViewModel model = new DocumentoViewModel();
            if (documento != null)
            {
                model.IdDocumento = documento.idDocumento;
                model.FechaDocumento = documento.fechaContabilizacion;
                model.Asunto = documento.asunto;
                //TODO: model.DocumentState =  documento.estaAprobado;
                model.IdCentroCosto1 = documento.idCentroCostos1;
                model.CentroCosto1 = documento.SAPCentroCostos?.PrcName;

                model.IdCentroCosto2 = documento.idCentroCostos2;
                model.CentroCosto1 = documento.SAPCentroCostos1?.PrcName;

                model.IdCentroCosto3 = documento.idCentroCostos3;
                model.CentroCosto1 = documento.SAPCentroCostos2?.PrcName;

                model.IdCentroCosto4 = documento.idCentroCostos4;
                model.CentroCosto1 = documento.SAPCentroCostos3?.PrcName;

                model.IdCentroCosto5 = documento.idCentroCostos5;
                model.CentroCosto1 = documento.SAPCentroCostos4?.PrcName;

                model.IdMetodoPago = documento.idMetodoPago;
                model.MetodoPago = documento.SAPMetodoPago?.Descript;

                model.IdMoneda = documento.idMoneda;
                model.Moneda = documento.SAPMoneda?.CurrName;

                model.IdProveedorSolicitante = documento.idSAPProveedorSolicitante;
                model.ProveedorSolicitante = documento.SAPProveedor.CardName;

                model.Motivo = documento.motivoDetalle;

                documento.DocumentoDetalle.ToList().ForEach(x => model.ListDetalle.Add(GetDocumentoDetalleViewModel(x)));
            }
            else
            {
                model.FechaDocumento = DateTime.Now;
            }

            FillLists(dataContext, ref model);
            return model;
        }

        public void FillLists(DataContext dataContext, ref DocumentoViewModel model)
        {
            model.ListCentrosCosto = new SAPCentroCostosLogic().GetJsonListSAPCentroCostos(dataContext);
            model.ListMetodosPago = new SAPMetodoPagoLogic().GetJsonListSAPMetodoPago(dataContext);
            model.ListMonedas = new SAPMonedaLogic().GetJsonListSAPMonedas(dataContext);
            model.ListProveedorSolicitante = new SAPProveedorLogic().GetJSONListSAPProveedors(dataContext);
        }

        public void FillLists(DataContext dataContext, ref DocumentoDetalleViewModel model)
        {
            model.ListCentrosCosto = new SAPCentroCostosLogic().GetJsonListSAPCentroCostos(dataContext);
            model.ListMonedas = new SAPMonedaLogic().GetJsonListSAPMonedas(dataContext);
            model.ListProveedores = new SAPProveedorLogic().GetJSONListSAPProveedors(dataContext);
            model.ListConceptos = new ConceptoLogic().JsonListConceptos(dataContext);
        }

        public Int32? AddUpdateDocumento(DataContext dataContext, DocumentoViewModel model)
        {
            return new DocumentoDataAccess().SaveDocumento(dataContext, model);
        }

        public void AddUpdateDocumentoDetalle(DataContext dataContext, DocumentoDetalleViewModel model, ref List<DocumentoDetalleViewModel> list)
        {
            //Validate properties
            Boolean modelIsValid = true;
            Boolean esUpdate = model.IdDocumentoDetalle == null ? true : false;

            if (!esUpdate)
            {
                model.IdDocumentoDetalle = -+new Guid().GetHashCode();
            }
            else
            {
                var modelToRemove = list.Where(x => x.IdDocumentoDetalle == model.IdDocumentoDetalle).FirstOrDefault();
                list.Remove(modelToRemove);
            }

            list.Add(model);

            FillLists(dataContext, ref model);

            if (!modelIsValid)
                throw new Exception("Error de validación: ");//TODO: Handle errors
        }

        public DocumentoDetalleViewModel GetDocumentoDetalleViewModel(DocumentoDetalle x)
        {
            return new DocumentoDetalleViewModel()
            {
                Correlativo = x.correlativoDoc,
                Fecha = x.fechaDocumento,
                IdCentroCosto1 = x.idCentroCosto1,
                IdCentroCosto2 = x.idCentroCosto2,
                IdCentroCosto3 = x.idCentroCosto3,
                IdCentroCosto4 = x.idCentroCosto4,
                IdCentroCosto5 = x.idCentroCosto5,
                IdConcepto = x.idConcepto,
                IdDocumentoDetalle = x.idDocumentoDetalle,
                IdMonedaDocumento = x.idMonedaOriginal,
                IdMonedaDocumentoSAP = x.idMonedaDoc,
                IdProveedor = x.idProveedor,
                ImporteAfecta = x.montoAfecto,
                ImporteIGV = x.montoIGV,
                ImporteNoAfecta = x.montoNoAfecto,
                ImporteTotal = x.montoTotal,
            };
        }

        public DocumentoDetalleViewModel DetalleDocumentoViewModel(DataContext dataContext)
        {
            DocumentoDetalleViewModel model = new DocumentoDetalleViewModel();
            model.ListCentrosCosto = new SAPCentroCostosLogic().GetJsonListSAPCentroCostos(dataContext);
            model.ListConceptos = new ConceptoLogic().JsonListConceptos(dataContext);
            model.ListMonedas = new SAPMonedaLogic().GetJsonListSAPMonedas(dataContext);
            model.ListProveedores = new SAPProveedorLogic().GetJSONListSAPProveedors(dataContext);
            model.ListTipoDocumento = new List<JsonEntity>();
            return model;
        }
    }
}
