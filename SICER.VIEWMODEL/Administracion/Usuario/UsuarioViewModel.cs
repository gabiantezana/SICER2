using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Rol;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
    public class UsuarioViewModel
    {
        public int? UsuarioId { get; set; }

        public string Password { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public int RolId { get; set; }
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

        #region Roles por Usuario

        //public IEnumerable<RolViewModel> RolList { get; set; } = new List<RolViewModel>();
        //public IEnumerable<UsuarioRolesViewModel> RolUserList { get; set; } = new List<UsuarioRolesViewModel>();

        public IEnumerable<MODEL.Rol> RolList { get; set; } = new List<MODEL.Rol>();
        public IEnumerable<UsuarioRoles> RolUserList { get; set; } =  new List<UsuarioRoles>();

        #endregion


    }
}
