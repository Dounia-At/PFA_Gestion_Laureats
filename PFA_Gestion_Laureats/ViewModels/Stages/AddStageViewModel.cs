using PFA_Gestion_Laureats.Models;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Stages
{
    public class AddStageViewModel
    {
        [Required]
        public string Intitulé_poste { get; set; }
        [Required]
        [Display(Name = "Date de début  ")]
        [DataType(DataType.Date)]
        public DateTime Date_Debut { get; set; }
        //[Compare(nameof(Date_Debut), ErrorMessage = "La date de fin doit être postérieure à la date de début")]
        [Required]
        [Display(Name = "Date de fin")]
        
        [DataType(DataType.Date)]
        public DateTime Date_Fin { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Entreprise { get; set; }
        public int EntrepriseId { get; set; }
        [Required]
        public int EtudiantId { get; set; }

    }
}
