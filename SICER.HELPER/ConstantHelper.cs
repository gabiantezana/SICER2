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
        public static int NUMEROFILASPORPAGINA = 1;
        public static object MENSAJE_TABLA_VACIA = "No se encontraron registros.";

        public const string MENSAJE_EXITO = "Operación  realizada exitosamente.";
        public const string MENSAJE_ERROR = "Ocurrió un error.";
        public const string SEPARADOR_NOMBRE_DESCRIPCION_SELECT = " - ";
        public const string PASSWORD_DEFAULT = "1234";
        public const string CODIGOROLSUPERADMINISTRADOR = "SUPERADMIN";

        public const string CAJACHICA = "CAJACHICA";
        public const string ENTREGARENDIR = "ENTREGARENDIR";
        public const string REEMBOLSO = "REEMBOLSO";

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

            public static class Sincronizacion
            {
                private const string PARENTPREFIX = "SINCRONIZACION";
            }

            public static class Documento
            {
                private const string PARENTPREFIX = "DOCUMENTO.";

                public static class CajaChica
                {
                    private const string PREFIX = PARENTPREFIX + "CAJACHICA.";

                    public const string LISTAR = PREFIX + "LISTAR";
                    public const string CREAR = PREFIX + "CREAR";
                    public const string APROBAR = PREFIX + "APROBAR";
                    public const string LIQUIDAR = PREFIX + "LIQUIDAR";
                }

                public static class EntregaRendir
                {
                    private const string PREFIX = PARENTPREFIX + "ENTREGARENDIR.";

                    public const string LISTAR = PREFIX + "LISTAR";
                    public const string CREAR = PREFIX + "CREAR";
                    public const string APROBAR = PREFIX + "APROBAR";
                    public const string LIQUIDAR = PREFIX + "LIQUIDAR";
                }

                public static class Reembolso
                {
                    private const string PREFIX = PARENTPREFIX + "REEMBOLSO.";

                    public const string LISTAR = PREFIX + "LISTAR";
                    public const string CREAR = PREFIX + "CREAR";
                    public const string APROBAR = PREFIX + "APROBAR";
                    public const string LIQUIDAR = PREFIX + "LIQUIDAR";
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
