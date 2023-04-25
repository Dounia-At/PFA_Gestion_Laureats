using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Annonces
{
    public class UpdateAnnonceViewModel
    {
        public int Id { get; set; }

        public string Titre { get; set; }
        public string Description { get; set; }


        public IFormFile? Photo { get; set; }

        [Display(Name = "Email de Reception ")]
        public string Email_Reception { get; set; }

        public DateTime Date_limite_Deposer { get; set; }
        
        [Required]
        public string Entreprise { get; set; }
        public int EntrepriseId { get; set; }
        public UpdateAnnonceViewModel() { }
        public UpdateAnnonceViewModel(Annonce annonce)
        {
            Id = annonce.Id;
            Titre = annonce.Titre;
            //this. = annonce.Photo;
            Description = annonce.Description;
            Email_Reception = annonce.Email_Reception;
            Date_limite_Deposer = annonce.Date_limite_Deposer;

        }
    }
}
