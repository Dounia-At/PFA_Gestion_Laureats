using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
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
        [Display(Name = "Entreprise")]
        public int EntrepriseId { get; set; }
        public UpdateAnnonceViewModel() { }
        public UpdateAnnonceViewModel(Annonce annonce) { 
            this.Id = annonce.Id;
            this.Titre = annonce.Titre;
            //this. = annonce.Photo;
            this.Description = annonce.Description;
            this.Email_Reception = annonce.Email_Reception;
            this.Date_limite_Deposer = annonce.Date_limite_Deposer;
            
        }
    }
}
