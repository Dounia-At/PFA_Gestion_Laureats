using PFA_Gestion_Laureats.Services;
using PFA_Gestion_Laureats.ViewModels.Annonces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
{
    public class Annonce
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string ?Photo { get; set; }
        public string Email_Reception { get; set; }
        public DateTime Date_limite_Deposer { get; set; }
        public DateTime Date_Creation { get; set; }

        public string Nature { get; set; }
        public bool Remuniration { get; set; }
        public IList<Postulation>? postulations { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur? utilisateur { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise? entreprise { get; set; }
        public IList<AnnonceTechnologie>? AnnonceTechnologies { get; set; }
        public Annonce() { }
        public Annonce(AddAnnonceViewModel amv) { 
            this.Titre = amv.Titre;
            
            this.Description= amv.Description;
            this.Email_Reception=amv.Email_Reception;
            this.Date_limite_Deposer=amv.Date_limite_Deposer;
            this.Date_Creation=DateTime.Now;
            this.EntrepriseId= amv.EntrepriseId;
            this.Remuniration = amv.Remuniration;
            this.Nature = amv.Nature;
        }
        
        

    }
}
