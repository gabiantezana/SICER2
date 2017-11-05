using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class Rol
    {
        public Rol() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolId { get; set; }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        #region Virtual Lists

        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<VistaRol> VistaRol { get; set; }

        #endregion
    }
}
