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
            var usuarioLogueado = DataContext.Context.Usuario.Find(usuarioId);
            var nivelesDeAprobacionDeUsuario = usuarioLogueado.UsuarioNivelAprobacion
                .Where(x => x.NivelAprobacion.TipoDocumentoId == (int)documentType
                            && x.UsuarioId == usuarioLogueado.UsuarioId)
                .Select(x => x.NivelAprobacion.NumeroNivel);


            var principalQuery = DataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)documentType
                                                                          && x.SubTipoDocumento == (int)subType
                                                                          && x.Usuario.AreaId ==
                                                                          usuarioLogueado.AreaId);


            var queryDocumentosCreados = principalQuery.Where(x => x.CreacionUsuarioid == usuarioLogueado.UsuarioId);

            var queryAprobacionesHechas = DataContext.Context.DocumentoEstadosAuditoria.Where(x => x.UsuarioId == usuarioLogueado.UsuarioId).Select(x => x.Documento);

            var queryAprobacionesPendientes = principalQuery.Where(x =>
                                                            x.Estado == (int)DocumentState.Pendiente
                                                            && nivelesDeAprobacionDeUsuario.ToList().Contains((x.NivelAprobacion ?? 0) + 1));


            var finalQuery = queryDocumentosCreados.Concat(queryAprobacionesHechas).Concat(queryAprobacionesPendientes);
            finalQuery = finalQuery.Distinct();

            return finalQuery;
        }

        public IEnumerable<Documento> GetRendicionesList(int? documentoAperturaId, string filter)
        {
            var query = DataContext.Context.Documento.Where(x => x.AperturaDocumentoId == documentoAperturaId
                                                                 && x.AperturaDocumentoId != null);
            return query;
            var usuarioLogueadoId = DataContext.Session.GetIdUsuario();

            query = query.Where(x => x.CreacionUsuarioid == usuarioLogueadoId && x.Estado == (int)DocumentState.None);
        }

        public Documento GetEntity(int? documentoId)
        {
            return DataContext.Context.Documento.Find(documentoId);
        }

        public void AddUpdateApertura(DocumentoViewModel model)
        {
            try
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
                documento.Error = string.Empty;
                documento.MotivoRechazo = model.MotivoRechazo;
                documento.AperturaDocumentoId = null;


                documento.OstcCode = model.OstcCode;
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

                if (isUpdate)
                    DataContext.Context.Entry(documento);
                else
                    DataContext.Context.Documento.Add(documento);

                DataContext.Context.SaveChanges();

                if (isUpdate && (documento.Estado != model.Estado || documento.NivelAprobacion != model.NivelAprobacion))
                {
                    var documentoEstadosAuditoria = new DocumentoEstadosAuditoria()
                    {
                        UsuarioId = DataContext.Session.GetIdUsuario().Value,
                        DocumentoId = documento.DocumentoId,
                        Estado = documento.Estado,
                        NumeroNivel = documento.NivelAprobacion,
                        FechaAprobacion = DateTime.Now,
                    };

                    DataContext.Context.DocumentoEstadosAuditoria.Add(documentoEstadosAuditoria);
                    DataContext.Context.SaveChanges();
                }

                documento.Estado = model.Estado;
                documento.NivelAprobacion = model.NivelAprobacion;
                DataContext.Context.Entry(documento);
                DataContext.Context.SaveChanges();

                string sapError = null;
                if (model.MigrateToSap)
                {
                    try
                    {
                        MigrateToSap();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(SapException))
                            sapError = SapDataAccess.GetLastSapError(DataContext.Company).ErrorCode + " " +
                                       SapDataAccess.GetLastSapError(DataContext.Company).ErrorMessage;
                        else
                            sapError = ex.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(sapError))
                {
                    documento.Error = sapError;
                    DataContext.Context.Entry(documento);
                    DataContext.Context.SaveChanges();
                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void AddUpdateRendicion(DocumentoViewModel model)
        {
            if (model.AperturaDocumentoId == null) throw new Exception("Es necesario especificar el id de la apertura.");

            var documento = DataContext.Context.Documento.Find(model.DocumentoId);
            var isUpdate = documento != null;
            if (!isUpdate)
            {
                documento = new Documento
                {
                    AperturaDocumentoId = model.AperturaDocumentoId
                };
            }
            documento.TipoDocumentoId = DataContext.Context.Documento.Find(model.AperturaDocumentoId).TipoDocumentoId;
            documento.SubTipoDocumento = (int)DocumentSubType.Rendicion;
            documento.CreacionUsuarioid = DataContext.Session.GetIdUsuario().Value;
            documento.Estado = model.Estado;
            documento.C_1SapCentroCostoOcrCode = model.C_1SapCentroCostoOcrCode;
            documento.C_2SapCentroCostoOcrCode = model.C_2SapCentroCostoOcrCode;
            documento.C_3SapCentroCostoOcrCode = model.C_3SapCentroCostoOcrCode;
            documento.C_4SapCentroCostoOcrCode = model.C_4SapCentroCostoOcrCode;
            documento.C_5SapCentroCostoOcrCode = model.C_5SapCentroCostoOcrCode;
            documento.SapIndicatorCode = model.SapIndicatorCode;
            documento.Serie = model.Serie;
            documento.Correlativo = model.Correlativo;
            documento.CreacionFecha = DateTime.Now;
            documento.SapConceptoCode = model.SapConceptoCode;

            documento.FechaDocumento = model.FechaDocumento;
            documento.FechaSolicitud = model.FechaSolicitud;
            documento.FechaContabilizacion = model.FechaContabilizacion;
            documento.SapBusinessPartnerCardCode = model.SapBusinessPartnerCardCode;
            documento.SapMonedaDocCurrCode = model.SapMonedaDocCurrCode;
            documento.OstcCode = model.OstcCode;

            if (isUpdate)
                DataContext.Context.Entry(documento);
            else
                DataContext.Context.Documento.Add(documento);

            DataContext.Context.SaveChanges();

            if (model.MigrateToSap)
                MigrateToSap();
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

        public void MigrateToSap()
        {
            //TODO:
            // throw new NotImplementedException();
        }

        #region Rendicion

        #endregion

    }
}
