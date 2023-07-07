using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
{
    public class Technologie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Libelle { get; set; }
        public string? Logo { get; set; }
        [NotMapped]
        [ImageExtentionValidation(new string[] { ".png", ".jpg", ".jpeg", ".svg" }, ErrorMessage = "l'extention doit être png, svg, jpg ou jpeg")]
        public IFormFile? Pic { get; set; }
        public IList<AnnonceTechnologie>? AnnonceTechnologies { get; set; }
    }
}
