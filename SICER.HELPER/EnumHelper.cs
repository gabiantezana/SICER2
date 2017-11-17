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
    }

    public enum SyncEntity
    {
        /// <summary>
        /// CONCEPTOS
        /// </summary>
        @MSS_SICER_CCPT = 0,

        /// <summary>
        /// CONFIGURACIÓN DE CUENTAS
        /// </summary>
        @MSS_SICER_CONF = 1,

        /// <summary>
        /// BUSINESS PARTNERS
        /// </summary>
        OCRD = 2,

        /// <summary>
        /// MONEDAS
        /// </summary>
        OCRN = 3,

        /// <summary>
        /// INDICATORS
        /// </summary>
        OICD = 4,

        /// <summary>
        /// CENTRO COSTOS
        /// </summary>
        OOCR = 5,

        /// <summary>
        /// TIPOS DE CAMBIO
        /// </summary>
        ORTT = 6,
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

    public enum ModoVistaDocumento
    {
        Ver = 0,
        Crear = 1,
        Modificar = 2,
        Aprobar = 3
    }
}

