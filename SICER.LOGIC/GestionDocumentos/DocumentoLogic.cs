﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public class DocumentoLogic
    {
        #region GetList

        public static ListDocumentoViewModel GetListDocumentoViewModel(DataContext dataContext, DocumentType documentType, string query, int? page)
        {
            var model = new ListDocumentoViewModel()
            {
                PagedList = GetPagedList(dataContext, documentType, query, page),
                Filter = string.Empty,
                ListDefault = new List<Documento>(),
                DocumentType =  documentType
            };
            return model;
        }

        private static IEnumerable<Documento> GetQuery(DataContext dataContext, DocumentType documentType, string filter = null)
        {
            return new DocumentoDataAccess(dataContext).GetList(documentType, DocumentSubType.Apertura, dataContext.Session.GetIdUsuario(), filter);
        }

        public static IPagedList<Documento> GetPagedList(DataContext dataContext, DocumentType documentType, string filter, int? page)
        {
            return GetQuery(dataContext, documentType, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

        public static IEnumerable<JsonEntity> GetFilterJsonList(DataContext dataContext, DocumentType documentType, string filter)
        {
            //TODO:
            var query = GetQuery(dataContext, documentType, filter);
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

        public static DocumentoViewModel GetApertura(DataContext dataContext, int? documentoId)
        {
            var entity = new DocumentoDataAccess(dataContext).GetEntity(documentoId);
            var model = GetViewModelFromEntity(dataContext, entity);
            model.SubTipoDocumento = (int)DocumentSubType.Apertura;
            model.ModoVistaDocumento = GetModoVistaDocumento(dataContext, model.DocumentoId);


            //Carga rendiciones
            if (entity?.Documento1 != null)
            {
                foreach (var rendicion in entity?.Documento1.ToList())
                {
                    model.RendicionList.Add(rendicion.ConvertTo(typeof(DocumentoViewModel)));
                }
            }

            //Fill data inicial 
            if (documentoId == null)
                FillDataInicial(dataContext, ref model);

            FillJLists(dataContext, ref model);
            return model;
        }

        #endregion

        #region Save

        public static void AddUpdateApertura(DataContext dataContext, DocumentoViewModel model)
        {
            ValidarCamposDocumento(dataContext, model);
            var esNuevoDocumento = !model.DocumentoId.HasValue;
            if (esNuevoDocumento)
            {
                model.SubTipoDocumento = (int)DocumentSubType.Apertura;
                model.Codigo = GenerateCode(dataContext, model.DocumentType);
                model.SapIndicatorCode = SapIndicatorLogic.GetList(dataContext, null).FirstOrDefault(x => x.Code == ConfigLogic.GetCONFIGValue(dataContext, ConstantHelper.CONFIG.DOC_APER_INDICATOR))?.Code;
                model.Estado = (int)DocumentState.Pendiente;
                model.NivelAprobacion = NivelAprobacionLogic.GetFirstNivelAprobacion(dataContext, model.DocumentType);
            }
            new DocumentoDataAccess(dataContext).AddUpdate(model);
        }

        public static void ApproveApertura(DataContext dataContext, DocumentoViewModel model)
        {
            var nivelPendiente = GetNivelPendienteAprobacionFromApeprtura(dataContext, model.DocumentoId);
            if (nivelPendiente == null) throw new Exception("El documento no tiene ningún nivel de aprobación pendiente");

            ValidaSiUsuarioTieneNivelAprobacionRequerido(dataContext, nivelPendiente.Value);
            model.NivelAprobacion = nivelPendiente;

            var ultimoNivelAprobacion = NivelAprobacionLogic.GetLastNivelAprobacion(dataContext, model.DocumentType);
            if (nivelPendiente == ultimoNivelAprobacion)
            {
                model.Estado = (int)DocumentState.Aprobado;
                model.MigrateToSap = true;
            }
            AddUpdateApertura(dataContext, model);
        }

        #endregion

        #region Helper

        private static void FillDataInicial(DataContext dataContext, ref DocumentoViewModel model)
        {
            if (model.SubTipoDocumento == (int)DocumentSubType.Apertura)
            {
                model.ModoVistaDocumento = GetModoVistaDocumento(dataContext, model.DocumentoId);
                model.FechaSolicitud = DateTime.Now;
                model.FechaDocumento = DateTime.Now;
                model.FechaContabilizacion = DateTime.Now;
                model.MontoAfecto = 0;
                model.MontoNoAfecto = 0;
                model.MontoIgv = 0;
                model.CreacionUsuarioid = dataContext.Session.GetIdUsuario();

                //VALIDA
                var sapBusinessPartnerCardCode = UsuarioLogic.GetUsuario(dataContext, dataContext.Session.GetIdUsuario()).SapBusinessPartnerCardCode;
                if (string.IsNullOrEmpty(sapBusinessPartnerCardCode)) throw new Exception("No tiene ningún proveedor configurado para crear documentos.");
                model.SapBusinessPartnerCardCode = sapBusinessPartnerCardCode;

            }
        }

        private static DocumentoViewModel GetViewModelFromEntity(DataContext dataContext, Documento entity)
        {
            DocumentoViewModel model = entity?.ConvertTo(typeof(DocumentoViewModel)) ?? new DocumentoViewModel();
            return model;
        }

        public static void FillJLists(DataContext dataContext, ref DocumentoViewModel model)
        {
            //Fill select
            model.UsuarioJList = UsuarioLogic.GetUsuariosJsonList(dataContext, model.CreacionUsuarioid);
            model.AsociadaSapCuentaContableJList = SapCuentaContableLogic.GetJList(dataContext, null);
            model.GastoSapCuentaContableJList = SapCuentaContableLogic.GetJList(dataContext, null);

            if (!string.IsNullOrEmpty(model.SapBusinessPartnerCardCode))
                model.SapBusinessPartnerJList = SapBusinessPartnerLogic.GetJList(dataContext, model.SapBusinessPartnerCardCode);
            else
                model.SapBusinessPartnerJList = new List<JsonEntityTwoString>();



            model.SapConceptoJList = SapConceptoLogic.GetJList(dataContext, null);
            model.SapIndicatorJList = SapIndicatorLogic.GetJList(dataContext, null);
            model.SapMonedaJList = SapMonedaLogic.GetJList(dataContext, null);
            model.SubTipoDocumentoJList = null;
            model.C_1SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 1);
            model.C_2SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 2);
            model.C_3SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 3);
            model.C_4SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 4);
            model.C_5SapCentroCostoJList = SapCentroCostoLogic.GetJList(dataContext, null, 5);
        }

        private static ModoVistaDocumento GetModoVistaDocumento(DataContext dataContext, int? documentoId)
        {
            ModoVistaDocumento modoVistaDocumento = ModoVistaDocumento.Ver;

            var documento = new DocumentoDataAccess(dataContext).GetEntity(documentoId);
            if (documento == null) return ModoVistaDocumento.Crear;


            var userCanApprove = UserCanApproveDocument(dataContext, documentoId);
            var userIsCreator = UserIsCreator(dataContext, documentoId);

            switch ((DocumentState)documento.Estado)
            {
                case DocumentState.None:
                case DocumentState.Aprobado:
                    break;
                case DocumentState.Pendiente:
                    if (userCanApprove)
                        modoVistaDocumento = ModoVistaDocumento.Aprobar;
                    break;
                case DocumentState.Rechazado:
                    if (userIsCreator)
                        modoVistaDocumento = ModoVistaDocumento.Modificar;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return modoVistaDocumento;
        }

        private static string GenerateCode(DataContext dataContext, DocumentType documentType)
        {
            var newCode = string.Empty;
            var concat = 0000000000000000;
            var lastNumber = 0;
            string lastCode = dataContext.Context.Documento.Where(x => x.TipoDocumentoId == (int)documentType).OrderBy(x => x.DocumentoId).FirstOrDefault()?.Codigo;
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
            var proximoNivelAprobacion = GetNivelPendienteAprobacionFromApeprtura(dataContext, documentoId);
            if (proximoNivelAprobacion == null) return false;
            int? usuarioId = dataContext.Session.GetIdUsuario();
            var usuarioNivelAprobacion = dataContext.Context.UsuarioNivelAprobacion.FirstOrDefault(x => x.NivelAprobacion.NumeroNivel == proximoNivelAprobacion && x.UsuarioId == usuarioId);
            return usuarioNivelAprobacion != null;
        }

        private static bool UserIsCreator(DataContext dataContext, int? documentoId)
        {
            return dataContext.Context.Documento.Find(documentoId)?.CreacionUsuarioid == dataContext.Session.GetIdUsuario();
        }

        public static int? GetNivelPendienteAprobacionFromApeprtura(DataContext dataContext, int? documentoId)
        {

            var documento = dataContext.Context.Documento.Find(documentoId);
            if (documento == null) return null;

            //Si el documento no existe, escribe el primer nivel de apertura.
            var nivelesDeAprobacion = dataContext.Context.NivelAprobacion
                .Where(x => x.TipoDocumentoId == documento.TipoDocumentoId).Select(x => x.NumeroNivel).ToList();
            nivelesDeAprobacion.Sort();

            int? proximoNivel;
            switch ((DocumentState)documento.Estado)
            {
                case DocumentState.None:
                case DocumentState.Aprobado:
                case DocumentState.Rechazado:
                    return null;
                case DocumentState.Pendiente:
                    proximoNivel = nivelesDeAprobacion.FirstOrDefault(x => x > (documento.NivelAprobacion == null ? -1 : documento.NivelAprobacion));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return proximoNivel;
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
