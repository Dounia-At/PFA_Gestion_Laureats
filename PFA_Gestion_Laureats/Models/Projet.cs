using PFA_Gestion_Laureats.ViewModels.Projets;

namespace PFA_Gestion_Laureats.Models
{
    public class Projet
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime Date_Debut { get; set; }
        public DateTime Date_Fin { get; set; }
        public string Description { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }
        public Projet() { }
        public Projet(AddProjetViewModel amv)
        {
            this.Nom = amv.Nom;

            this.Description = amv.Description;
            this.Date_Debut = amv.Date_Debut;
            this.Date_Fin = amv.Date_Fin;
          
            this.EtudiantId = amv.EtudiantId;
        }
    }
}
