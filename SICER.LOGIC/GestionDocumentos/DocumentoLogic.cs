using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.DATAACCESS.GestionDocumentos;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.LOGIC.Administracion;
using SICER.LOGIC.Sync;
using SICER.MODEL;
using SICER.VIEWMODEL.GestionDocumentos;

namespace SICER.LOGIC.GestionDocumentos
{
    public static class DocumentoLogic
    {
        #region GetList

        public static ListDocumentoViewModel GetListDocumentoViewModel(DataContext dataContext, DocumentType documentType, string query, int? page)
        {
            var model = new ListDocumentoViewModel()
            {
                PagedList = GetOpeningPagedList(dataContext, documentType, query, page),
                Filter = string.Empty,
                ListDefault = new List<Documento>(),
                DocumentType = documentType
            };
            return model;
        }

        private static IEnumerable<DocumentoViewModel> GetOpeningList(DataContext dataContext, DocumentType documentType, string filter = null)
        {
            var defaultList = new DocumentoDataAccess(dataContext).GetOpeningList(documentType, filter);

            return defaultList.Select(item => GetViewModelFromEntity(dataContext, item)).ToList();
        }

        public static IPagedList<DocumentoViewModel> GetOpeningPagedList(DataContext dataContext, DocumentType documentType, string filter, int? page)
        {
            return GetOpeningList(dataContext, documentType, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

        public static IEnumerable<DocumentoViewModel> GetExpenditureList(DataContext dataContext, int? aperturaId, string filter)
        {
            var query = new DocumentoDataAccess(dataContext).GetExpenditureList(aperturaId, filter);
            var list = new List<DocumentoViewModel>();
            foreach (var rendicion in query.ToList())
            {
                var rendicionModel = GetExpenditure(dataContext, rendicion.AperturaDocumentoId, rendicion.DocumentoId);
                list.Add(rendicionModel);
            }
            return list;
        }

        public static IEnumerable<JsonEntity> GetFilterJsonList(DataContext dataContext, DocumentType documentType, string filter)
        {
            //TODO:
            var query = GetOpeningList(dataContext, documentType, filter);
            if (!string.IsNullOrEmpty(filter))
            {
                /*
                query = filtro.ToLower().Split(' ').Aggregate(query,
                    (current, token) => current.Where(x => x.Apellidos.ToLower().Contains(token)
                                         || x.Nombres.ToLower().Contains(token)
                                         || x.UserName.ToLower().Contains(token)));*/
            }
            var jsonEntities = query?.Select(x => new JsonEntity()
            {/*
                id = x.UsuarioId,
                text = x.GetNombreCompleto(),*/
            });

            return jsonEntities;
        }

        #endregion

        #region GetEntity

        public static DocumentoViewModel GetOpening(DataContext dataContext, int? documentoId)
        {
            if (!UserHasSupplier(dataContext))
                throw new CustomException("No tiene ningún proveedor configurado para gestionar documentos.");

            var model = GetViewModelFromEntity(dataContext, new DocumentoDataAccess(dataContext).GetEntity(documentoId));
            model.UserCanAddRendicion = UserCanAddRendicion(dataContext, model.DocumentoId);

            //Carga rendiciones
            model.RendicionList = GetExpenditureList(dataContext, model.DocumentoId, null);

            //Fill data inicial 
            if (documentoId == null)
                FillDataInicialOpening(dataContext, ref model);

            FillJLists(dataContext, ref model);
            return model;
        }

        public static DocumentoViewModel GetExpenditure(DataContext dataContext, int? aperturaDocumentoId, int? documentoId)
        {
            var entity = new DocumentoDataAccess(dataContext).GetEntity(documentoId);
            var model = GetViewModelFromEntity(dataContext, entity);

            model.ModosVistaDocumentoList = GetModoVistaDocumento(dataContext, documentoId);

            if (documentoId == null)
                FillDataInicialExpenditure(dataContext, ref model, aperturaDocumentoId);

            model.DocumentType = (DocumentType)model.TipoDocumentoId;

            FillJLists(dataContext, ref model);
            return model;
        }

        #endregion

        #region Save

        public static void AddUpdateDocument(DataContext dataContext, DocumentoViewModel model, SubmitType submitType)
        {
            if (model.SubTipoDocumento != null)
            {
                var documentSubType = (DocumentSubType)model.SubTipoDocumento;

                switch (submitType)
                {
                    case SubmitType.Save:
                        AddUpdateDocument(dataContext, model, documentSubType);
                        break;
                    case SubmitType.Send:
                        if (model.TipoDocumentoId == (int) DocumentType.CajaChica)
                        {
                            model.Estado = (int) DocumentState.Aprobado;
                            model.MigrateToSap = true;
                        }
                        else
                            model.Estado = (int)DocumentState.Pendiente;
                        AddUpdateDocument(dataContext, model, documentSubType);
                        break;
                    case SubmitType.Approve:
                        ApproveDocument(dataContext, model);
                        break;
                    case SubmitType.Refuse:
                        RefuseDocument(dataContext, model.DocumentoId, model.MotivoRechazo);
                        break;
                    case SubmitType.ResendToSap:
                        //TODO:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(submitType), submitType, null);
                }
            }
            else
                throw new Exception("Can't recognize the property DocumentSubType for this document.");
        }

        private static void AddUpdateDocument(DataContext dataContext, DocumentoViewModel model, DocumentSubType documentSubType)
        {
            switch (documentSubType)
            {
                case DocumentSubType.Apertura:
                    AddUpdateOpening(dataContext, model);
                    break;
                case DocumentSubType.Rendicion:
                    AddUpdateExpenditure(dataContext, model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentSubType), documentSubType, null);
            }
        }

        private static void AddUpdateOpening(DataContext dataContext, DocumentoViewModel model)
        {
            ValidarCamposDocumento(dataContext, model);

            if (!model.DocumentoId.HasValue)
                model.Codigo = GenerateCode(dataContext, model.DocumentType);

            new DocumentoDataAccess(dataContext).AddUpdateDocument(model);
        }

        private static void AddUpdateExpenditure(DataContext dataContext, DocumentoViewModel model)
        {
            ValidarCamposDocumento(dataContext, model);
            new DocumentoDataAccess(dataContext).AddUpdateDocument(model);
        }

        public static void ApproveDocument(DataContext dataContext, DocumentoViewModel model)
        {
            var nivelPendiente = GetNivelPendienteAprobacion(dataContext, model.DocumentoId);
            if (nivelPendiente == null) throw new Exception("El documento no tiene ningún nivel de aprobación pendiente");

            ValidaSiUsuarioTieneNivelAprobacionRequerido(dataContext, nivelPendiente.Value);
            model.NivelAprobacion = nivelPendiente;

            var ultimoNivelAprobacion = NivelAprobacionLogic.GetLastNivelAprobacion(dataContext, model.DocumentType);
            if (nivelPendiente == ultimoNivelAprobacion)
            {
                model.Estado = (int)DocumentState.Aprobado;
                model.MigrateToSap = true;
            }

            new DocumentoDataAccess(dataContext).AddUpdateDocument(model);
        }

        public static void RefuseDocument(DataContext dataContext, int? documentoId, string message)
        {
            new DocumentoDataAccess(dataContext).RechazarApertura(documentoId, message);
        }

        #endregion

        #region Helper

        private static void FillDataInicialOpening(DataContext dataContext, ref DocumentoViewModel model)
        {


            model.SubTipoDocumento = (int)DocumentSubType.Apertura;
            model.SapIndicatorCode = SapIndicatorLogic.GetList(dataContext, null).FirstOrDefault(x => x.Code == ConfigLogic.GetCONFIGValue(dataContext, ConstantHelper.CONFIG.DOC_APER_INDICATOR))?.Code;//TODO:

            model.FechaSolicitud = DateTime.Now;
            model.FechaDocumento = DateTime.Now;
            model.FechaContabilizacion = DateTime.Now;
            model.CreacionUsuarioid = dataContext.Session.GetIdUsuario();
            model.SapBusinessPartnerCardCode = UsuarioLogic.GetUsuario(dataContext, dataContext.Session.GetIdUsuario())?.SapBusinessPartnerCardCode;
        }

        private static void FillDataInicialExpenditure(DataContext dataContext, ref DocumentoViewModel model, int? aperturaDocumentoId)
        {

            var aperturaDocumento = new DocumentoDataAccess(dataContext).GetEntity(aperturaDocumentoId);
            if (aperturaDocumento == null)
                aperturaDocumento = dataContext.Context.Documento.Find(model.AperturaDocumentoId);

            if (aperturaDocumento == null) throw new Exception("No se encontró el documento de apertura.");

            //---------------------------Hierarchy data from Opening---------------------------
            model.AperturaDocumentoId = aperturaDocumento.DocumentoId;
            model.TipoDocumentoId = aperturaDocumento.TipoDocumentoId;
            //---------------------------Hierarchy data from Opening--------------------------

            model.SubTipoDocumento = (int)DocumentSubType.Rendicion;
            model.FechaSolicitud = DateTime.Now;
            model.FechaDocumento = DateTime.Now;
            model.FechaContabilizacion = DateTime.Now;
            model.CreacionUsuarioid = dataContext.Session.GetIdUsuario();
            model.Estado = (int)DocumentState.None;
        }

        private static DocumentoViewModel GetViewModelFromEntity(DataContext dataContext, Documento entity)
        {
            DocumentoViewModel model = entity?.ConvertTo(typeof(DocumentoViewModel)) ?? new DocumentoViewModel();
            return model;
        }

        public static void FillJLists(DataContext dataContext, ref DocumentoViewModel model)
        {
            //Fill select
            model.ModosVistaDocumentoList = GetModoVistaDocumento(dataContext, model.DocumentoId);
            model.UsuarioJList = UsuarioLogic.GetUsuariosJsonList(dataContext, model.CreacionUsuarioid);

            if (!string.IsNullOrEmpty(model.SapBusinessPartnerCardCode))
                model.SapBusinessPartnerJList = SapBusinessPartnerLogic.GetJList(dataContext, model.SapBusinessPartnerCardCode);
            else
                model.SapBusinessPartnerJList = new List<JsonEntityTwoString>();


            model.OstcJList = OstcLogic.GetJList(dataContext, null);
            model.SapConceptoJList = SapConceptoLogic.GetJList(dataContext, null);
            model.SapIndicatorJList = SapIndicatorLogic.GetJList(dataContext, null);
            model.SapMonedaJList = SapMonedaLogic.GetJList(dataContext, null);
            model.C_1SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 1);
            model.C_2SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 2);
            model.C_3SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 3);
            model.C_4SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 4);
            model.C_5SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 5);
        }

        private static IEnumerable<ModoVistaDocumentoApertura> GetModoVistaDocumento(DataContext dataContext, int? documentoId)
        {
            var list = new List<ModoVistaDocumentoApertura>();
            var documento = new DocumentoDataAccess(dataContext).GetEntity(documentoId);

            if (documento == null)
            {
                list.Add(ModoVistaDocumentoApertura.Crear);
                list.Add(ModoVistaDocumentoApertura.Modificar);
                list.Add(ModoVistaDocumentoApertura.ModificarYEnviar);
                return list;
            }

            var userCanApprove = UserCanApproveDocument(dataContext, documentoId);
            var userIsCreator = UserIsCreator(dataContext, documentoId);

            switch ((DocumentState)documento.Estado)
            {
                case DocumentState.None:
                    if (userIsCreator)
                    {
                        list.Add(ModoVistaDocumentoApertura.Crear);
                        list.Add(ModoVistaDocumentoApertura.ModificarYEnviar);
                    }
                    break;
                case DocumentState.Pendiente:
                    if (userCanApprove)
                        list.Add(ModoVistaDocumentoApertura.ModificarYAprobar);
                    break;
                case DocumentState.Aprobado:
                    break;
                case DocumentState.Rechazado:
                    if (userIsCreator)
                        list.Add(ModoVistaDocumentoApertura.ModificarYEnviar);
                    break;
                case DocumentState.AprobadoConErroresDeMigracion:
                    if (userIsCreator)
                        list.Add(ModoVistaDocumentoApertura.ModificarYReenviarASap);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return list;
        }

        private static string GenerateCode(DataContext dataContext, DocumentType documentType)
        {
            var newCode = string.Empty;
            var concat = 0000000000000000;
            var lastNumber = 0;
            string lastCode = dataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)documentType).OrderByDescending(x => x.DocumentoId).FirstOrDefault()?.Codigo;
            string prefix = null;

            switch (documentType)
            {
                case DocumentType.CajaChica:
                    prefix = "CC";
                    break;
                case DocumentType.EntregaARendir:
                    prefix = "ER";
                    break;
                case DocumentType.Reembolso:
                    prefix = "RE";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null);
            }
            if (lastCode != null)
            {
                try
                {
                    lastNumber = Convert.ToInt32(lastCode.Replace(prefix, ""));
                }
                catch
                {
                }
            }
            var newNumber = "0000000000000000000" + (Convert.ToInt32(lastNumber) + 1);
            newCode = prefix + newNumber.Substring(newNumber.Length - 6);
            return newCode;

        }

        private static bool UserCanApproveDocument(DataContext dataContext, int? documentoId)
        {
            var proximoNivelAprobacion = GetNivelPendienteAprobacion(dataContext, documentoId);
            if (proximoNivelAprobacion == null) return false;
            int? usuarioId = dataContext.Session.GetIdUsuario();
            var usuarioNivelAprobacion = dataContext.Context.UsuarioNivelAprobacion.FirstOrDefault(x => x.NivelAprobacion.NumeroNivel == proximoNivelAprobacion && x.UsuarioId == usuarioId);
            return usuarioNivelAprobacion != null;
        }

        private static bool UserIsCreator(DataContext dataContext, int? documentoId)
        {
            return dataContext.Context.Documento.Find(documentoId)?.CreacionUsuarioid == dataContext.Session.GetIdUsuario();
        }

        public static int? GetNivelPendienteAprobacion(DataContext dataContext, int? documentoId)
        {
            var documento = dataContext.Context.Documento.Find(documentoId);
            if (documento == null) return null;

            var nivelesDeAprobacion = dataContext.Context.NivelAprobacion
                .Where(x => x.TipoDocumentoId == documento.TipoDocumentoId).Select(x => x.NumeroNivel).ToList();
            nivelesDeAprobacion.Sort();

            int? proximoNivel;
            switch ((DocumentState)documento.Estado)
            {
                case DocumentState.None:
                case DocumentState.Aprobado:
                case DocumentState.Rechazado:
                case DocumentState.AprobadoConErroresDeMigracion:
                    return null;
                case DocumentState.Pendiente:
                    proximoNivel = nivelesDeAprobacion.FirstOrDefault(x => x > (documento.NivelAprobacion ?? -1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (proximoNivel == null)
                throw new CustomException("El documento no tiene ningún nivel de aprobación pendiente.");
            return proximoNivel;
        }

        public static bool UserCanAddRendicion(DataContext dataContext, int? documentoId)
        {
            var canAddRendicion = false;
            var documento = new DocumentoDataAccess(dataContext).GetEntity(documentoId);
            if (documento != null)
            {
                if (documento.Estado == (int)DocumentState.Aprobado
                    && documento.CreacionUsuarioid == dataContext.Session.GetIdUsuario())
                    canAddRendicion = true;
                //TODO:  VALIDACIÓN DE MONTOS:
            }
            return canAddRendicion;
        }

        public static bool UserHasSupplier(DataContext dataContext)
        {
            var sapBusinessPartnerCardCode = UsuarioLogic.GetUsuario(dataContext, dataContext.Session.GetIdUsuario()).SapBusinessPartnerCardCode;
            return !string.IsNullOrEmpty(sapBusinessPartnerCardCode);
        }


        public static bool UserCAnApproveRendicion(DataContext dataContext, int? documentoId)
        {
            var documento = new DocumentoDataAccess(dataContext).GetEntity(documentoId);
            if (documento == null) return false;

            var apertura = new DocumentoDataAccess(dataContext).GetEntity(documento.AperturaDocumentoId);
            if (apertura == null) return false;

            if (apertura.Estado == (int)DocumentState.Aprobado
                && documento.Estado == (int)DocumentState.Pendiente
                && UserCanApproveDocument(dataContext, apertura.DocumentoId)
                )
                return true;

            return false;
        }

        #endregion

        #region Validaciones

        public static void ValidaSiUsuarioTieneNivelAprobacionRequerido(DataContext dataContext, int nivelPendiente)
        {
            var usuarioId = dataContext.Session.GetIdUsuario();
            var nivelAprobacion = dataContext.Context.UsuarioNivelAprobacion.FirstOrDefault(x => x.UsuarioId == usuarioId && x.NivelAprobacion.NumeroNivel == nivelPendiente);

            var usuarioTienePermiso = nivelAprobacion != null;

            if (!usuarioTienePermiso)
                throw new CustomException("No tiene el nivel de aprobación requerido para aprobar este documento.");

        }

        private static void ValidarCamposDocumento(DataContext dataContext, DocumentoViewModel model)
        {
            //TODO:
        }

        #endregion

    }
}
