using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Concepto
{
    public class ConceptoViewModel
    {
        public Int32? IdConcepto { get; set; }

        [Required]
        [Display(Name = "Cuenta contable")]
        public Int32? IdAccount { get; set; }
        public List<JsonEntity> ListSAPAccounts { get; set; } = new List<JsonEntity>();

        public String Nombre { get; set; }

        public String Descripcion { get; set; }
    }
}
