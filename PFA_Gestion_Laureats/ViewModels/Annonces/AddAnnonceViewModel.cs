using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Annonces
{
    public class AddAnnonceViewModel
    {
        [Required]
        [MaxLength(30)]
        public string Titre { get; set; }
        public string Description { get; set; }


        public IFormFile? Photo { get; set; }
        [Required]
        [Display(Name = "Email de Reception ")]
        public string Email_Reception { get; set; }
        [Required]
        public DateTime Date_limite_Deposer { get; set; }
        
        [Required]
        public string Entreprise { get; set; }
        public int EntrepriseId { get; set; }

    }
}
