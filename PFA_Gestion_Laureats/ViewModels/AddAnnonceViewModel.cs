using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
{
    public class AddAnnonceViewModel
    {
        [Required]
        public string Titre { get; set; }
        public string Description { get; set; }

       
        public IFormFile ?Photo { get; set; }
        [Required]
        [Display(Name = "Email de Reception ")]
        public string Email_Reception { get; set; }
        [Required]
        public DateTime Date_limite_Deposer { get; set; }
        [Display(Name = "Entreprise")]
        public int EntrepriseId { get; set; }

    }
}
