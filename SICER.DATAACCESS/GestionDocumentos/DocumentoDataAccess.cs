using System;
using System.Collections.Generic;
using System.Linq;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.GestionDocumentos;

namespace SICER.DATAACCESS.GestionDocumentos
{
    public class DocumentoDataAccess
    {
        private DataContext DataContext { get; set; }

        public DocumentoDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        //TODO: Filters
        public IEnumerable<Documento> GetList(DocumentType documentType, DocumentSubType subType, int? usuarioId, string filter)
        {

            return DataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)documentType
                                                         && x.SubTipoDocumento == (int)subType
                                                         //&& x.Usuario.AreaId == 
                                                         );
        }

        public Documento GetEntity(int? documentoId)
        {
            return DataContext.Context.Documento.Find(documentoId);
        }

        public int AddUpdate(DocumentoViewModel model)
        {
            var documento = DataContext.Context.Documento.Find(model.DocumentoId);
            var isUpdate = documento != null;

            if (!isUpdate)
            {
                documento = new Documento();
                documento.CreacionFecha = DateTime.Now;
                documento.CreacionUsuarioid = DataContext.Session.GetIdUsuario();
                documento.Estado = model.Estado;
                documento.NivelAprobacion = null;
            }
            else
            {
                documento.ModificacionFecha = DateTime.Now;
                documento.ModificacionUsuarioid = DataContext.Session.GetIdUsuario();
                documento.NivelAprobacion = model.NivelAprobacion;
            }

            documento.TipoDocumentoId = (int)model.DocumentType;
            documento.SubTipoDocumento = model.SubTipoDocumento;
            documento.Codigo = model.Codigo;
            documento.Asunto = model.Asunto;
            documento.Motivo = model.Motivo;
            documento.FechaDocumento = DateTime.Now;
            documento.FechaSolicitud = DateTime.Now;
            documento.FechaContabilizacion = DateTime.Now;
            documento.Serie = model.Serie;
            documento.Correlativo = model.Correlativo;
            documento.Estado = model.Estado;
            documento.Error = string.Empty;
            documento.MotivoRechazo = model.MotivoRechazo;
            documento.AperturaDocumentoId = null;
            documento.MontoNoAfecto = model.MontoNoAfecto;
            documento.MontoAfecto = model.MontoAfecto;
            documento.MontoIgv = model.MontoIgv;


            documento.SapBusinessPartnerCardCode = model.SapBusinessPartnerCardCode;
            documento.SapConceptoCode = model.SapConceptoCode;
            documento.SapIndicatorCode = model.SapIndicatorCode;
            documento.SapMonedaDocCurrCode = model.SapMonedaDocCurrCode;
            documento.AsociadaSapCuentaContableCode = model.AsociadaSapCuentaContableCode;
            documento.GastoSapCuentaContableCode = model.GastoSapCuentaContableCode;
            documento.C_1SapCentroCostoOcrCode = model.C_1SapCentroCostoOcrCode;
            documento.C_2SapCentroCostoOcrCode = model.C_2SapCentroCostoOcrCode;
            documento.C_3SapCentroCostoOcrCode = model.C_3SapCentroCostoOcrCode;
            documento.C_4SapCentroCostoOcrCode = model.C_4SapCentroCostoOcrCode;
            documento.C_5SapCentroCostoOcrCode = model.C_5SapCentroCostoOcrCode;

            foreach (var rendicion in model.RendicionList)
            {
                AddUpdate(rendicion);
            }

            if (isUpdate)
                DataContext.Context.Entry(documento);
            else
                DataContext.Context.Documento.Add(documento);

            DataContext.Context.SaveChanges();

            if (model.MigrateToSap)
                MigrateToSap();

            return documento.DocumentoId;
        }

        public void ChangeNivelAprobacionDocumentoApertura(int? documentoId, int? nivelAprobacion)
        {

            var documento = DataContext.Context.Documento.Find(documentoId);
            if (documento == null) throw new CustomException("No se encontró el documento en la base de datos");
            documento.NivelAprobacion = nivelAprobacion;
            DataContext.Context.Entry(documento);
            DataContext.Context.SaveChanges();
        }

        public void MigrateToSap()
        {
            //TODO:
            // throw new NotImplementedException();
        }
        #region Rendicion

        public IEnumerable<Documento> GetListRendiciones(int documentoId)
        {
            return DataContext.Context.Documento.Where(x => x.AperturaDocumentoId == documentoId);
        }

        #endregion

    }
}
