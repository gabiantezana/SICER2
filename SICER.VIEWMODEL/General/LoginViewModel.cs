using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SICER.HELPER;

namespace SICER.VIEWMODEL.General
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Codigo { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Compañía")]
        public int CompanyId { get; set; }
        public IEnumerable<JsonEntity> CompanyJList { get; set; }

    }
}