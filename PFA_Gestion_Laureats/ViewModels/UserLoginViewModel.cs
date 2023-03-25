using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Ce champs est obligatoire")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Ce champs est obligatoire")]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}
