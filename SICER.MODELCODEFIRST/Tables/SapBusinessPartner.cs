using System.ComponentModel.DataAnnotations;

namespace SICER.MODELCODEFIRST.Tables
{
    public class SapBusinessPartner
    {
        [Key]
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }
}