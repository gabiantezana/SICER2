using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Config
{
    public class ConfigViewModel
    {
        [Display(Name = "Valor requerido")]
        [Required(ErrorMessage = "Identificador requerido")]
        [MaxLength(20)]
        public string id { get; set; }

        [Required(ErrorMessage = "Valor requerido")]
        [Display(Name = "Valor del parámetro")]
        public string valor { get; set; }

        [Required(ErrorMessage = "Valor requerido")]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
    }
}
