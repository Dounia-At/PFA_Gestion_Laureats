using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
{
    public class UserViewModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Tel { get; set; }
        public string Email { get; set; }
        public string Titre_Profil { get; set; }
        public string Adresse { get; set; }
        public string Login { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }
        public string URL_Photo_Profil { get; set; }

        [ProfilExtentionValidation(new string[] { ".png", ".jpg", ".jpeg" }, ErrorMessage = "l'extention doit être png, jpg ou jpeg")]

        public IFormFile Photo_Profil { get; set; }
    }
}
