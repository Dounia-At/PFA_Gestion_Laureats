using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Annonces
{
    public class AddPostulerViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [ImageExtentionValidation(new string[] { ".pdf", ".docx" }, ErrorMessage = "Vérifier l'extention de fichier")]
        public IFormFile CV { get; set; }
    }
}
