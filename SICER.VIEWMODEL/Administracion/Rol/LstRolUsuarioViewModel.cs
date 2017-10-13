using SICER.MODEL;
using System;
using System.Collections.Generic;

namespace SICER.VIEWMODEL.Administracion.Rol
{
    public class LstRolUsuarioViewModel
    {
        public List<Vistas> LstVistas { get; set; }
        public List<VistasGrupo> LstGrupoVistas { get; set; }
        public List<VistasRol> LstVistasRol { get; set; }
        public Int32 idRol { get; set; }
    }
}
