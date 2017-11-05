using System.ComponentModel.DataAnnotations;

namespace SICER.MODELCODEFIRST.Tables
{
    public class SapMoneda
    {
        [Key]
        public string DocCurrCode { get; set; }
        public string CurrName { get; set; }
        public string Locked { get; set; }
    }
}