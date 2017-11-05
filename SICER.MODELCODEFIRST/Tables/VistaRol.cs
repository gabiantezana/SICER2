using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class VistaRol
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VistaRolId { get; set; }

        public bool Estado { get; set; }

        #region Foreign Keys

        public int VistaId { get; set; }
        public Vista Vista { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        #endregion
    }
}
