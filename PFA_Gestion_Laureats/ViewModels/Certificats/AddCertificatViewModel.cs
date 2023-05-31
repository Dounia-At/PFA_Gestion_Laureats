using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Certificats
{
    public class AddCertificatViewModel
    {
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Organisation { get; set; }
        [Required]
        [Display(Name = "Date d'emission")]
        public DateTime Date_Emission { get; set; }
        
        [Display(Name = "Date d'expiration ")]
        //[Compare("Date_Emission", ErrorMessage = "La date d'expiration doit être postérieure à la date d'emission")]
        public DateTime ?Date_Expiration { get; set; }
        [DataType(DataType.Url)]
        public string ?Url { get; set; }
        [Required]
        public int EtudiantId { get; set; }
    }
}
