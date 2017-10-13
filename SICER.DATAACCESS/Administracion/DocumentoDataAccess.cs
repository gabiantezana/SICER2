using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class DocumentoDataAccess
    {
        //TODO: Filters
        public List<Documento> GetListDocumento(DataContext dataContext, DocumentType documentType)
        {
            return dataContext.Context.Documento.Where(x => x.idTipoOrigen == (Int32)documentType).ToList();
        }

        public Documento GetDocumento(DataContext dataContext, Int32? idDocumento)
        {
            return dataContext.Context.Documento.Find(idDocumento);
        }


        public Int32? SaveDocumento(DataContext dataContext, DocumentoViewModel model)
        {
            Boolean  isUpdate = false;
            Documento documento = dataContext.Context.Documento.Find(model.IdDocumento);

            if (documento != null)
                isUpdate = true;

            if (!isUpdate)
                documento = new Documento();

            documento.asunto = model.Asunto;
            documento.comentario = String.Empty;
            documento.estaAprobado = 0;
            documento.estadoMigracion = 0;
            documento.fechaContabilizacion = DateTime.Now;
            documento.fechaCreacion = DateTime.Now;
            documento.fechaSolicitud = DateTime.Now;
            documento.fechaUltimoUpdate = DateTime.Now;
            documento.idCentroCostos1 = model.IdCentroCosto1;
            documento.idCentroCostos2 = model.IdCentroCosto2;
            documento.idCentroCostos3 = model.IdCentroCosto3;
            documento.idCentroCostos4 = model.IdCentroCosto4;
            documento.idCentroCostos5 = model.IdCentroCosto5;
            documento.idMetodoPago = model.IdMetodoPago;
            documento.idMoneda = model.IdMoneda;
            documento.idSAPProveedorSolicitante = model.IdProveedorSolicitante;
            documento.idTipoOrigen = 1;
            documento.idUsuarioCreacion = dataContext.Session.GetIdUsuario();
            documento.idUsuarioUltimoUpdate = dataContext.Session.GetIdUsuario();
            documento.montoGastado = 0;
            documento.montoSolicitado = 0;
            documento.motivoDetalle = model.Motivo;

            model.ListDetalle.ForEach(x => AddUpdateDocumentLine(dataContext, x, ref documento));

            if (isUpdate)
                dataContext.Context.Entry(documento);
            else
                dataContext.Context.Documento.Add(documento);

            dataContext.Context.SaveChanges();
            return documento.idDocumento;
        }

        private void AddUpdateDocumentLine(DataContext dataContext, DocumentoDetalleViewModel model, ref Documento documento)
        {
            DocumentoDetalle detalle = documento.DocumentoDetalle.Where(x => x.idDocumentoDetalle == model.IdDocumentoDetalle).FirstOrDefault();
            Boolean esUpdate = false;

            if (detalle != null)
                esUpdate = true;

            if(!esUpdate)
                detalle = new DocumentoDetalle();

            detalle.idConcepto = model.IdConcepto;
            detalle.idDocumento = documento.idDocumento;
            detalle.idMonedaOriginal = model.IdMonedaDocumento;
            detalle.idMonedaDoc = model.IdMonedaDocumentoSAP;
            detalle.idProveedor = model.IdProveedor;
            detalle.idTipoDocumentoOrigen = 1;
            detalle.idUsuarioCreacion = dataContext.Session.GetIdUsuario();
            detalle.idUsuarioUltimoUpdate = dataContext.Session.GetIdUsuario();
            detalle.montoAfecto = model.ImporteAfecta;
            //detalle.montoDoc = model.ImporteTotal;
            detalle.montoIGV = model.ImporteIGV;
            detalle.montoNoAfecto = model.ImporteNoAfecta;
            detalle.montoTotal = model.ImporteTotal;
            detalle.idCentroCosto1 = model.IdCentroCosto1;
            detalle.idCentroCosto2 = model.IdCentroCosto2;
            detalle.idCentroCosto3 = model.IdCentroCosto3;
            detalle.idCentroCosto4 = model.IdCentroCosto4;
            detalle.idCentroCosto5 = model.IdCentroCosto5;
            //detalle.idCentroCosto6 = model.IdCentroCosto6;
            detalle.idConcepto = model.IdConcepto;
            detalle.serieDoc = model.Serie;
            //detalle.tasaCambio = model.TasaDeCambio;

            if (esUpdate)
                dataContext.Context.Entry(detalle);
            else
                dataContext.Context.DocumentoDetalle.Add(detalle);
        }
    }
}
