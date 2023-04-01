using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
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

        public bool Convention { get; set; }
        [ImageExtentionValidation(new string[] { ".png", ".jpg", ".jpeg" }, ErrorMessage = "l'extention doit être png, jpg ou jpeg")]
        public IFormFile? Photo { get; set; }
        public EntrepriseViewModel()
        {

        }
        public EntrepriseViewModel(int Id, string Nom, string Pays, string? Ville,
                            string Adresse, string Logo,
                            string Description, bool Convention)
        {
            this.Id = Id;
            this.Nom = Nom;
            this.Pays = Pays;
            this.Ville = Ville;
            this.Logo = Logo;
            this.Adresse = Adresse;
            this.Description = Description;
            this.Convention = Convention;


        }
    }
}
