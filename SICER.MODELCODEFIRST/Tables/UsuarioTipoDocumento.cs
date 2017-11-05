using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class UsuarioTipoDocumento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioTipoDocumentoId { get; set; }

        public int CantidadMaximaDocumentos { get; set; }

        #region Foreign Keys

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }

        #endregion

    }
}
