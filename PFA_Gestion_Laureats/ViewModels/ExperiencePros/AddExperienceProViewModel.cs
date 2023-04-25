using PFA_Gestion_Laureats.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.ExperiencePros
{
    public class AddExperienceProViewModel
    {
        [Required]
        
        public string Post { get; set; }
        [Required]
        [Display(Name = "Type d'emploi ")]
        public string Type_Emploi { get; set; }
        [Required]
        public bool Etat { get; set; }
       
        [Required]
        public string Entreprise { get; set; }
        public int EntrepriseId { get; set; }
        [Required]
       
        public int EtudiantId { get; set; }
    }
}
