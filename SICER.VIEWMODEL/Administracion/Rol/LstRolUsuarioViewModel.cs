using System;
using System.Collections.Generic;
using SICER.MODEL;

namespace SICER.VIEWMODEL.Administracion.Rol
{
    public class LstRolUsuarioViewModel
    {
        public List<Vista> LstVistas { get; set; }
        public List<GrupoVista> LstGrupoVistas { get; set; }
        public List<VistaRol> LstVistasRol { get; set; }
        public Int32 RolId { get; set; }
    }
}
