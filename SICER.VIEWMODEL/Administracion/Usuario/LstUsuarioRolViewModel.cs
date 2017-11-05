using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
    public class LstUsuarioRolViewModel 
    {
        public List<Vista> LstVistas { get; set; }
        public List<GrupoVista> LstGrupoVistas { get; set; }
        public List<VistaRol> LstVistasRol { get; set; }

        public int IdRol { get; set; }
    }
}
