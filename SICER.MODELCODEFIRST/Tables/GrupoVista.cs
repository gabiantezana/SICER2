using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class GrupoVista
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int GrupoVistaId { get; set; }
        public string Nombre { get; set; }

        #region Virtual Lists

        public IEnumerable<Vista> Vistas { get; set; }

        #endregion
    }

}