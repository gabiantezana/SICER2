using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
    public class UsuarioViewModel
    {
        public int? UsuarioId { get; set; }

        public string Password { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public Int32 RolId { get; set; }
        public List<JsonEntity> RolJList { get; set; } = new List<JsonEntity>();

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string SapBusinessPartnerCardCode { get; set; }
    }
}
