using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Area
{
    public class AreaViewModel
    { 
        public int? AreaId { get; set; }

        [Required]
        public string Descripcion { get; set; }
    }
}
