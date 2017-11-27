using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
        public IEnumerable<Documento> GetOpeningList(DocumentType documentType, string filter)
        {
            var usuarioLogueado = DataContext.Context.Usuario.Find(DataContext.Session.GetIdUsuario());
            var nivelesDeAprobacionDeUsuario = usuarioLogueado.UsuarioNivelAprobacion
                .Where(x => x.NivelAprobacion.TipoDocumentoId == (int)documentType
                            && x.UsuarioId == usuarioLogueado.UsuarioId)
                .Select(x => x.NivelAprobacion.NumeroNivel);


            var principalQuery = DataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)documentType
                                                                          && x.SubTipoDocumento == (int)DocumentSubType.Apertura
                                                                          && x.Usuario.AreaId ==
                                                                          usuarioLogueado.AreaId);


            var queryDocumentosCreados = principalQuery.Where(x => x.CreacionUsuarioid == usuarioLogueado.UsuarioId);

            var queryAprobacionesHechas = DataContext.Context.DocumentoEstadosAuditoria.Where(x => x.UsuarioId == usuarioLogueado.UsuarioId
                                                                                              && x.Documento.SubTipoDocumento == (int)DocumentSubType.Apertura).Select(x => x.Documento);

            var queryAprobacionesPendientes = principalQuery.Where(x =>
                                                            x.Estado == (int)DocumentState.Pendiente
                                                            && nivelesDeAprobacionDeUsuario.ToList().Contains((x.NivelAprobacion ?? 0) + 1));


            var finalQuery = queryDocumentosCreados.Concat(queryAprobacionesHechas).Concat(queryAprobacionesPendientes);
            finalQuery = finalQuery.Distinct();

            return finalQuery;
        }

        public IEnumerable<Documento> GetExpenditureList(int? documentoAperturaId, string filter)
        {
            var openingDocumento = DataContext.Context.Documento.Find(documentoAperturaId);
            var _usuarioLogueado = DataContext.Context.Usuario.Find(DataContext.Session.GetIdUsuario());

            var list = new List<Documento>();
            if (openingDocumento == null) return list;

            var nivelesDeAprobacionDeUsuario = _usuarioLogueado.UsuarioNivelAprobacion
                .Where(x => x.NivelAprobacion.TipoDocumentoId == (int)openingDocumento.TipoDocumentoId
                            && x.UsuarioId == _usuarioLogueado.UsuarioId)
                .Select(x => x.NivelAprobacion.NumeroNivel);


            var principalQuery = DataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)openingDocumento.TipoDocumentoId
                                                                          && x.SubTipoDocumento == (int)DocumentSubType.Rendicion
                                                                          && x.Usuario.AreaId == _usuarioLogueado.AreaId);


            var queryDocumentosCreados = principalQuery.Where(x => x.CreacionUsuarioid == _usuarioLogueado.UsuarioId);

            var queryAprobacionesPendientes = principalQuery.Where(x =>
                x.Estado == (int)DocumentState.Pendiente
                && nivelesDeAprobacionDeUsuario.ToList().Contains((x.NivelAprobacion ?? 0) + 1));

            var queryAprobacionesHechas = DataContext.Context.DocumentoEstadosAuditoria.Where(x => x.UsuarioId == _usuarioLogueado.UsuarioId
                                                                                                   && x.Documento.SubTipoDocumento == (int)DocumentSubType.Rendicion
                                                                                                   && x.Documento.Documento2.DocumentoId == documentoAperturaId).Select(x => x.Documento);
            var query1 =
                DataContext.Context.DocumentoEstadosAuditoria.Where(x => x.UsuarioId == _usuarioLogueado.UsuarioId);
            var query2 = query1.Where(x => x.Documento.SubTipoDocumento == (int)DocumentSubType.Rendicion);
            var query3 = query2.Where(x => x.Documento.Documento2.DocumentoId == documentoAperturaId).Select(x => x.Documento);


            var finalQuery = queryDocumentosCreados.Concat(queryDocumentosCreados).Concat(queryAprobacionesPendientes).Concat(queryAprobacionesHechas);
            finalQuery = finalQuery.Distinct();
            return finalQuery;


            /*
            var apertura = DataContext.Context.Documento.Find(documentoAperturaId);
            if (apertura == null) return new List<Documento>();

            var usuarioLogueado = DataContext.Context.Usuario.Find(DataContext.Session.GetIdUsuario());

            var principalQuery = DataContext.Context.Documento.Where(x => x.SubTipoDocumento == (int) DocumentSubType.Rendicion
                                                                    && x.AperturaDocumentoId == documentoAperturaId
                                                                    && x.Estado != (int)DocumentState.None);

            var queryDocumentosSinEstadoDeUsuario = DataContext.Context.Documento.Where(x =>
                                                                    x.SubTipoDocumento == (int) DocumentSubType.Rendicion
                                                                    && x.AperturaDocumentoId == documentoAperturaId
                                                                    && x.Estado == (int) DocumentState.None
                                                                    && x.CreacionUsuarioid == usuarioLogueado.UsuarioId);

            var finalQuery = principalQuery.Concat(queryDocumentosSinEstadoDeUsuario);
            finalQuery = finalQuery.Distinct();

            return finalQuery;*/
        }

        public Documento GetEntity(int? documentoId)
        {
            return DataContext.Context.Documento.Find(documentoId);
        }

        public void AddUpdateDocument(DocumentoViewModel model)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var documento = DataContext.Context.Documento.Find(model.DocumentoId);
                    var isUpdate = documento != null;

                    if (!isUpdate)
                    {
                        documento = new Documento();
                        documento.CreacionFecha = DateTime.Now;
                        documento.CreacionUsuarioid = DataContext.Session.GetIdUsuario().Value;
                    }
                    else
                    {
                        documento.ModificacionFecha = DateTime.Now;
                        documento.ModificacionUsuarioid = DataContext.Session.GetIdUsuario();
                    }

                    documento.TipoDocumentoId = model.TipoDocumentoId;
                    documento.SubTipoDocumento = model.SubTipoDocumento;
                    documento.AperturaDocumentoId = model.AperturaDocumentoId;
                    documento.Codigo = model.Codigo;
                    documento.Asunto = model.Asunto;
                    documento.Motivo = model.Motivo;
                    documento.FechaDocumento = model.FechaDocumento;
                    documento.FechaSolicitud = model.FechaSolicitud;
                    documento.FechaContabilizacion = model.FechaContabilizacion;
                    documento.Serie = model.Serie;
                    documento.Correlativo = model.Correlativo;
                    documento.MotivoRechazo = model.MotivoRechazo;

                    documento.SapOstcCode = model.OstcCode;
                    documento.SapBusinessPartnerCardCode = model.SapBusinessPartnerCardCode;
                    documento.SapConceptoCode = model.SapConceptoCode;
                    documento.SapIndicatorCode = model.SapIndicatorCode;
                    documento.SapMonedaDocCurrCode = model.SapMonedaDocCurrCode;
                    documento.C_1SapCentroCostoOcrCode = model.C_1SapCentroCostoOcrCode;
                    documento.C_2SapCentroCostoOcrCode = model.C_2SapCentroCostoOcrCode;
                    documento.C_3SapCentroCostoOcrCode = model.C_3SapCentroCostoOcrCode;
                    documento.C_4SapCentroCostoOcrCode = model.C_4SapCentroCostoOcrCode;
                    documento.C_5SapCentroCostoOcrCode = model.C_5SapCentroCostoOcrCode;

                    if (isUpdate)
                        DataContext.Context.Entry(documento);
                    else
                        DataContext.Context.Documento.Add(documento);
                    DataContext.Context.SaveChanges();

                    // ------------------------GUARDA AUDITORÍAS DE ESTADOS------------------------
                    SaveAuditoriaEstados(model, isUpdate, ref documento);
                    //------------------------GUARDA AUDITORÍAS DE ESTADOS------------------------

                    //---------------------------------MIGRA A SAP---------------------------------
                    if (model.MigrateToSap)
                    {
                        var sapError = MigrateToSap(model);
                        if (!string.IsNullOrEmpty(sapError))
                        {
                            documento.Error = sapError;
                            documento.Estado = (int)DocumentState.AprobadoConErroresDeMigracion;
                            DataContext.Context.Entry(documento);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    //---------------------------------MIGRA A SAP---------------------------------

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void RechazarApertura(int? documentoId, string message)
        {
            var documento = DataContext.Context.Documento.Find(documentoId);
            if (documento == null) return;

            documento.MotivoRechazo = message;
            documento.Estado = (int)DocumentState.Rechazado;
            documento.NivelAprobacion = null;
            DataContext.Context.Entry(documento);

            var documentoEstadoAuditoria = new DocumentoEstadosAuditoria()
            {
                UsuarioId = DataContext.Session.GetIdUsuario(),
                DocumentoId = documento.DocumentoId,
                Estado = documento.Estado,
                NumeroNivel = null,
                FechaAprobacion = DateTime.Now,
            };

            DataContext.Context.DocumentoEstadosAuditoria.Add(documentoEstadoAuditoria);

            DataContext.Context.SaveChanges();
        }

        private void SaveAuditoriaEstados(DocumentoViewModel model, bool isUpdate, ref Documento documento)
        {
            if (documento.Estado != model.Estado
                || !isUpdate && model.Estado == 0
                || documento.NivelAprobacion != model.NivelAprobacion)
            {
                var documentoEstadosAuditoria = new DocumentoEstadosAuditoria()
                {
                    UsuarioId = DataContext.Session.GetIdUsuario().Value,
                    DocumentoId = documento.DocumentoId,
                    Estado = model.Estado,
                    NumeroNivel = model.NivelAprobacion,
                    FechaAprobacion = DateTime.Now,
                };

                DataContext.Context.DocumentoEstadosAuditoria.Add(documentoEstadosAuditoria);
            }

            documento.Estado = model.Estado;
            documento.NivelAprobacion = model.NivelAprobacion;
            DataContext.Context.SaveChanges();
        }

        private string MigrateToSap(DocumentoViewModel model)
        {
            //TODO: INSERT DATA INTO SAP
            string sapError = null;
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //if (ex.GetType() == typeof(SapException))
                //sapError = SapDataAccess.GetLastSapError(DataContext.Company).ErrorCode + " " +
                //          SapDataAccess.GetLastSapError(DataContext.Company).ErrorMessage;
                //else
                //    sapError = ex.ToString();
            }
            return sapError;
        }

    }
}
