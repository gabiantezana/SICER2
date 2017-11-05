using System.ComponentModel.DataAnnotations;

namespace SICER.MODELCODEFIRST.Tables
{
    public class SapMetodoPago
    {
        [Key]
        public string PayMethCode { get; set; }
        public string Descript { get; set; }
        public string Active { get; set; }
    }
}