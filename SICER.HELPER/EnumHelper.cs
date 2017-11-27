using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.HELPER
{
    public class EnumHelper
    {

    }
    public enum SessionKey
    {
        UsuarioId,
        UserName,
        NombresUsuario,
        Rol,
        NombreRol,
        Vistas,
        CompanyId,
        CompanyName,
    }

    public enum TempDataKey
    {
        ClientesCombosList,
        ClientesActiveTab,
        ReservasLstPointsInMap,
        ParametrosActiveTab,
        ConductoresActiveTab
    }

    public enum ActiveTab
    {
        InformacionParametros,
    }

    public enum AppRol //TODO: SYNC WITH DATABASE rolId
    {
        SUPERADMIN = 1,
        SISTEMAS = 36,
        ADMIN = 5,
        GESTORDOCUMENTOS = 7,
        //Creador,
        //Aprobador,
        //Contador
    }

    public enum RolNivel
    {
        Principal = 1,
        Secundario = 2,
    }

    public enum MessageType
    {
        Success,
        Warning,
        Info,
        Error
    }

    #region MODAL SIZE
    public enum ModalSize
    {
        Normal,
        Small,
        Large
    }
    #endregion

    public enum SyncType
    {
        Create,
        Update,
        Delete,
    }

    public enum DocumentType
    {
        CajaChica = 1,
        EntregaARendir = 2,
        Reembolso = 3,
    }

    public enum DocumentSubType
    {
        Apertura = 1,
        Rendicion = 2,
    }

    public enum DocumentState
    {
        None = 0,
        Pendiente = 1,
        Aprobado = 2,
        Rechazado = 3,
        AprobadoConErroresDeMigracion = 4
    }

    public enum QueryFileName
    {
        /// <summary>
        /// CONCEPTOS
        /// </summary>
        MSS_SICER_CCPT_GetList = 0,

        /// <summary>
        /// BUSINESS PARTNERS
        /// </summary>
        OCRD_GetList = 2,

        /// <summary>
        /// MONEDAS
        /// </summary>
        OCRN_GetList = 3,

        /// <summary>
        /// INDICATORS
        /// </summary>
        OICD_GetList = 4,

        /// <summary>
        /// CENTRO COSTOS
        /// </summary>
        OOCR_GetList = 5,

        /// <summary>
        /// TIPOS DE CAMBIO
        /// </summary>
        ORTT_GetList = 6,

        /// <summary>
        /// CÓDIGO DE IMPUESTOS
        /// </summary>
        OSTC_GetList =7,

        /// <summary>
        /// CONFIGURACIÓN DE CUENTAS
        /// </summary>
        MSS_SICER_AACT_GetList = 8,
    }

    public enum SourceType
    {
        Local = 1,
        Sap = 2
    }

    public enum DocumentView
    {
        Listar = 1,
        Crear = 2,
    }

    public enum ModoVistaDocumentoApertura
    {
        Ver = 0,
        Crear = 1,
        Modificar = 2,
        ModificarYEnviar = 3,
        ModificarYAprobar = 4,
        ModificarYReenviarASap = 5,
    }

    public enum SubmitType
    {
        Save,
        Send,
        Approve,
        Refuse,
        ResendToSap
    }

    public enum ModoVistaDocumentoRendicion 
    {
        Ver = 0, //No tiene ningún permiso 
        Crear = 1,
        Modificar = 2, //Puede modificarla (antes de haber sido enviada)
        Enviar = 2, //Puede enviarla a flujo
        Aprobar = 3, //Puede aprobarla o rechazarla
    }
}

