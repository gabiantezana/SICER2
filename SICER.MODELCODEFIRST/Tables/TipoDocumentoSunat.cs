using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class TipoDocumentoSunat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoDocumentoSunatId { get; set; }

        public string Nombre { get; set; }
        public string CodigoSunat { get; set; }

        #region Lists

        public virtual ICollection<Documento> Documentos { get; set; }
        public virtual ICollection<DocumentoRendicion> DocumentoRendicions { get; set; }

        #endregion
    }
}
