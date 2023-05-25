using Microsoft.AspNetCore.Mvc;
using PFA_Gestion_Laureats.Models;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Tests
{
    public class TestViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Date du test")]
        public DateTime Date_Test { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        [DataType(DataType.Time)]
        [Display(Name = "Heure du test")]
        public DateTime Heure_Test { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public string Description { get; set; }
        [Required]
        public string Entreprise { get; set; }

        [Required(ErrorMessage = "Champ obligatoire!")]
        public int EntrepriseId { get; set; }

        public TestViewModel()
        {

        }
        public TestViewModel(int? id, DateTime date_Test, DateTime heure_Test, string description, int entrepriseId)
        {
            Id = id;
            Date_Test = date_Test;
            Heure_Test = heure_Test;
            Description = description;
            EntrepriseId = entrepriseId;
        }
    }
}
