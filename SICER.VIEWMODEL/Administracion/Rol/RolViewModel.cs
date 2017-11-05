using System;
using System.ComponentModel.DataAnnotations;

namespace SICER.VIEWMODEL.Administracion.Rol
{
    public class RolViewModel
    {
        public int? RolId { get; set; }

        [Required(ErrorMessage="Campo requerido")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Descripcion { get; set; }
    }
}
