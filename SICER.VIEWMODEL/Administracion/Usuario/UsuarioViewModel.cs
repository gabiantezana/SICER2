using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Display(Name = "Área")]
        public int? AreaId { get; set; }
        public IEnumerable<JsonEntity> AreaJList { get; set; } = new List<JsonEntity>();

        [Required]
        [Display(Name = "Compañía")]
        public int? CompanyId { get; set; }
        public IEnumerable<JsonEntity> CompanyJList { get; set; } = new List<JsonEntity>();


        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [DisplayName("Proveedor SAP")]
        public string SapBusinessPartnerCardCode { get; set; }
        public List<JsonEntityTwoString> SapBusinessPartnerJList { get; set; } = new List<JsonEntityTwoString>();

        #region Roles por Usuario

        /// <summary>
        /// Listado de todos los roles que existen en la base de datos.
        /// </summary>
        public IEnumerable<MODEL.Rol> RolList { get; set; } = new List<MODEL.Rol>();

        /// <summary>
        /// Listado de roles asociados al usuario.
        /// </summary>
        public IEnumerable<UsuarioRoles> RolUserList { get; set; } =  new List<UsuarioRoles>();


        /// <summary>
        /// Listado de niveles de aprobación asociados al usuario.
        /// </summary>
        public IEnumerable<int> NivelAprobacionIdList { get; set; } = new List<int>();

        #endregion


    }
}
