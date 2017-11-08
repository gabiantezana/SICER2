﻿using System;
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

    public enum AppRol
    {
        SUPERADMIN,
        ADMIN,
        GESTORDOCUMENTOS,
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

    public enum DocumentState
    {
        Aprobado,
        Rechazado,
        EnObservacion,
    }
}

