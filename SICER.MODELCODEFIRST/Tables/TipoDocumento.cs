using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class TipoDocumento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoDocumentoId { get; set; }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
