using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Tel { get; set; }
        public string Email { get; set; }
        public string Titre_Profil { get; set; }
        public string Adresse { get; set; }      
        public string Login { get; set; }
        public string Password { get; set; }
        public string Photo_Profil { get; set; }
       
        public bool Isvalide { get; set; }
        public IList<Annonce>? annonces { get; set; }
        public IList<Message>? messagesEnvoyees { get; set; }
        public IList<Message>? BoitesReception { get; set; }

    }

}


