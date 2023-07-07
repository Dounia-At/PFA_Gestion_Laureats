using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire!")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire!")]
        public string Prenom { get; set; }
        public string? Tel { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire!")]
        [Display(Name = "Titre")]
        public string Titre_Profil { get; set; }
        [Required(ErrorMessage = "Champ Obligatoire!")]
        public string Adresse { get; set; }      
        [Required(ErrorMessage = "Champ Obligatoire!")]
        public string Login { get; set; }
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Champ Obligatoire!")]
        public string Password { get; set; }
        public string? Photo_Profil { get; set; }
        [DefaultValue(false)]
        public bool Isvalide { get; set; }
        [DefaultValue(false)]
        public bool IsComfirmed { get; set; }
        public bool IsVisibleTel { get; set; }
        public DateTime ?date_Login { get;set; }
        public DateTime ?date_Logout { get; set; }

        public IList<Annonce>? annonces { get; set; }
        public IList<Message>? messagesEnvoyees { get; set; }
        public IList<Message>? BoitesReception { get; set; }

        public string ?ConnectionId { get; set; }
    }

}


