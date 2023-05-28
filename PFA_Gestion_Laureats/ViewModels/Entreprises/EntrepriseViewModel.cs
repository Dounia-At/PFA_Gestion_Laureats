using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Entreprises
{
    public class EntrepriseViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Pays { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Ville { get; set; }
        public string? Adresse { get; set; }
        public string? Logo { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool Convention { get; set; }
        [ImageExtentionValidation(new string[] { ".png", ".jpg", ".jpeg" }, ErrorMessage = "l'extention doit être png, jpg ou jpeg")]
        public IFormFile? Photo { get; set; }
        public EntrepriseViewModel()
        {

        }
        public EntrepriseViewModel(Entreprise e)
        {
            this.Id = e.Id;
            this.Nom = e.Nom;
            this.Pays = e.Pays;
            this.Ville = e.Ville;
            this.Logo = e.Logo;
            this.Adresse = e.Adresse;
            this.Description =  e.Description;
            this.Convention = e.Convention;
        }
    }
}
