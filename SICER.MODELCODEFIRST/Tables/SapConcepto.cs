using System.ComponentModel.DataAnnotations;

namespace SICER.MODELCODEFIRST.Tables
{
    public class SapConcepto
    {
        [Key]
        public string U_Codigo { get; set; }
        public string U_CuentaContable { get; set; }
        public string U_Descripcion { get; set; }
    }
}
