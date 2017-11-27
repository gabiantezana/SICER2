using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SICER.HELPER
{
    public static class SessionHelper
    {
        #region Get & Set
        public static object Get(HttpSessionStateBase Session, SessionKey Key)
        {
            return Session[Key.ToString()];
        }
        public static object Get(HttpSessionState Session, SessionKey Key)
        {
            return Session[Key.ToString()];
        }
        public static void Set(this HttpSessionStateBase Session, SessionKey Key, object value)
        {
            Session[Key.ToString()] = value;
        }
        public static void Set(this HttpSessionState Session, SessionKey Key, object value)
        {
            Session[Key.ToString()] = value;
        }
        #endregion

        #region GetPermisosVista

        public static String[] GetPermisosVista(this HttpSessionStateBase Session)
        {
            return (String[])Get(Session, SessionKey.Vistas)?? new String[] { };
        }
        public static String[] GetPermisosVista(this HttpSessionState Session)
        {
            return (String[])Get(Session, SessionKey.Vistas)?? new String[] { };
        }

        #endregion

        #region GetRol
        public static AppRol GetRol(this HttpSessionStateBase Session)
        {
            return (AppRol)Get(Session, SessionKey.Rol);
        }
        public static AppRol GetRol(this HttpSessionState Session)
        {
            return (AppRol)Get(Session, SessionKey.Rol);
        }
        #endregion

        #region GetIdUsuario

        public static Int32? GetIdUsuario(this HttpSessionStateBase Session)
        {
            return (int?)Get(Session, SessionKey.UsuarioId);
        }
        public static int? GetIdUsuario(this HttpSessionState session)
        {
            return (int?)Get(session, SessionKey.UsuarioId);
        }

        #endregion

        #region GetCompanyName

        public static string GetCompanyName(this HttpSessionStateBase Session)
        {
            return (string)Get(Session, SessionKey.CompanyName);
        }
        public static string GetCompanyName(this HttpSessionState session)
        {
            return (string)Get(session, SessionKey.CompanyName);
        }

        #endregion


        #region GetCompanyId

        public static int? GetCompanyId(this HttpSessionStateBase Session)
        {
            return (int?)Get(Session, SessionKey.CompanyId);
        }
        public static int? GetCompanyId(this HttpSessionState session)
        {
            return (int?)Get(session, SessionKey.CompanyId);
        }

        #endregion

        #region GetUserName

        public static String GetUserName(this HttpSessionStateBase Session)
        {
            return (String)Get(Session, SessionKey.UserName);
        }
        public static String GetUserName(this HttpSessionState Session)
        {
            return (String)Get(Session, SessionKey.UserName);
        }

        #endregion

        #region GetNombresUsuario

        public static String GetNombresUsuario(this HttpSessionStateBase Session)
        {
            return (String)Get(Session, SessionKey.NombresUsuario);
        }
        public static String GetNombresUsuario(this HttpSessionState Session)
        {
            return (String)Get(Session, SessionKey.NombresUsuario);
        }

        #endregion
    }
}
