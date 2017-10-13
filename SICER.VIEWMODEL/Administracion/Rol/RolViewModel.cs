using System;
using System.ComponentModel.DataAnnotations;

namespace SICER.VIEWMODEL.Administracion.Rol
{
    public class RolViewModel
    {
        public Int32? idRol { get; set; }

        [Required(ErrorMessage="Campo requerido")]
        public String Codigo { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public String Nombre { get; set; }
    }
}
