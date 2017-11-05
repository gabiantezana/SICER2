using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class Vista
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VistaId { get; set; }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        #region Foreign Keys

        public int GrupoVistaId { get; set; }
        public GrupoVista GrupoVista { get; set; }

        #endregion

        #region Virtual Lists

        public IEnumerable<VistaRol> VistaRols { get; set; }

        #endregion
    }
}
