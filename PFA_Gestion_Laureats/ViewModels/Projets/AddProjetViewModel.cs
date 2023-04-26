using PFA_Gestion_Laureats.Models;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Projets
{
    public class AddProjetViewModel
    {
        [Required]
        [Display(Name = "Nom du Projet")]
        public string Nom { get; set; }
        [Required]
        [Display(Name = "Date de début  ")]
        public DateTime Date_Debut { get; set; }
        [Required]
        [Display(Name = "Date de fin")]
        //[Compare("Date_Debut", ErrorMessage = "La date de fin doit être postérieure à la date de début")]
        public DateTime Date_Fin { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]
        public int EtudiantId { get; set; }

    }
}
