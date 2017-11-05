using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class AprobacionDocumento
    {
        public AprobacionDocumento() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AprobacionDocumentoID { get; set; }

        #region Foreign Keys

        public int UsuarioTipoDocumentoID { get; set; }
        public virtual UsuarioTipoDocumento UsuarioTipoDocumento { get; set; }

        public int UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }

        #endregion
    }
}
