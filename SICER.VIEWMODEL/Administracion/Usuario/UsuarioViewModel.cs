using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
    public class UsuarioViewModel
    {
        public Int32? IdUsuario { get; set; }

        public String Password { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public Int32 IdRol { get; set; }
        public List<JsonEntity> LstRol { get; set; }

        [Required]
        public String Nombres { get; set; }

        [Required]
        public String Documento { get; set; }

        public String Correo { get; set; }

        public String Telefono { get; set; }

        public Boolean Estado { get; set; }

        public String Usuario { get; set; }

    }
}
