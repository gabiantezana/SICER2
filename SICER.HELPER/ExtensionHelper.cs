using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.HELPER
{
    public static class ExtensionHelper
    {
        public static string GetNombreCompleto(this Usuario usuario)
        {
            return usuario?.Nombres.ToSafeString() + " " + usuario?.Apellidos.ToSafeString();
        }



    }
}
