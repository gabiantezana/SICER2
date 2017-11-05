using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace SICER.MODELCODEFIRST.Tables
{
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public string UserName { get; set; }

        public byte[] Password { get; set; }


        #region Foreign Keys

        [Required]
        public int? RolId { get; set; }
        public Rol Rol { get; set; }

        public string SapBusinessPartnerCardCode { get; set; }
        public SapBusinessPartner SapBusinessPartner { get; set; }

        #endregion

        #region Virtual List

        public virtual ICollection<VistaRol> VistaRols { get; set; }

        public virtual ICollection<AprobacionDocumento> AprobacionDocumentos { get; set; }
        public virtual ICollection<DocumentoRendicion> CreacionDocumentoRendicions { get; set; }
        public virtual ICollection<DocumentoRendicion> ModificacionDocumentoRendicions { get; set; }
        public virtual ICollection<UsuarioTipoDocumento> UsuarioTipoDocumentos { get; set; }

        #endregion
    }
}
