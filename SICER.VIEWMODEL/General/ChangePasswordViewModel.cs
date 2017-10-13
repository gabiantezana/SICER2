using System.ComponentModel.DataAnnotations;

namespace SICER.VIEWMODEL.General
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(15, ErrorMessage = "Debe tener entre 6 a 15 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar el password")]
        [StringLength(15, ErrorMessage = "Debe tener entre 6 a 15 caracteres", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
