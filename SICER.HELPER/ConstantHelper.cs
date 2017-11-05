using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.HELPER
{
    public class ConstantHelper
    {
        public static readonly byte[] ENCRIPT_KEY = { 45, 12, 45, 78, 2, 45, 12, 65, 87, 12, 45, 32, 20, 58, 15, 36, 47, 85, 96, 20, 24, 23, 65, 24 };
        public static readonly byte[] ENCRIPT_METHOD = { 87, 10, 65, 35, 12, 66, 21, 65 };

        public static String DEFAULT_PASSWORD = "123456";

        public static String MENSAJE_EXITO = "Operación  realizada exitosamente.";
        public static String MENSAJE_ERROR = "Ocurrió un error.";
        public static String SEPARADOR_NOMBRE_DESCRIPCION_SELECT = " - ";
        public static string PASSWORD_DEFAULT ="1234";
        public static string CODIGOROLSUPERADMINISTRADOR = "Superadmin";

        public static class Rol
        {
            public const String SUPERADMIN = "SUPERADMIN";

        }

        public static class Vistas
        {
            public static class Administracion
            {
                private const string PARENTPREFIX = "ADMINISTRACION.";

                public static class Usuario
                { 
                    private const string PREFIX = PARENTPREFIX + "USUARIO.";

                    public const string LISTAR = PREFIX + "LISTAR";
                    public const string CREAR = PREFIX + "CREAR";
                }
                public static class Rol
                {
                    private const string PREFIX = PARENTPREFIX + "ROL.";

                    public const string LISTAR = PREFIX + "LISTAR";
                    public const string CREAR = PREFIX + "CREAR";
                }
            }
        }

        public static class Area
        {
            public const String ADMINISTRACION = "ADMINISTRACION";
        }

        public static class WEBCONFIG
        {
            public const String PORTAL_NAME = "PORTAL_NAME";
        }
    }
}
