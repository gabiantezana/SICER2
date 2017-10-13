using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
    class LstUsuarioRolViewModel
    {
        public List<Vistas> LstVistas { get; set; }
        public List<VistasGrupo> LstVistasGrupo { get; set; }
        public List<VistasRol> LstVistasRol { get; set; }
        public Int32 idRol { get; set; }
    }
}