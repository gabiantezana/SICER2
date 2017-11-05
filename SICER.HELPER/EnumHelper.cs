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
        IdUsuario,
        UserName,
        NombresUsuario,
        Rol,
        NombreRol,
        Vistas,
    }

    public enum AppRol
    {
        Superadmin,
        Administrador,
        //Creador,
        //Aprobador,
        //Contador
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

    public enum DocumentState
    {
        Aprobado,
        Rechazado,
        EnObservacion,
    }
}

