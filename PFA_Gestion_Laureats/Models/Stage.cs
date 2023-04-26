using PFA_Gestion_Laureats.ViewModels.Stages;

namespace PFA_Gestion_Laureats.Models
{
    public class Stage
    {
        public int Id { get; set; }
        public string Intitulé_poste { get; set; }
        public DateTime Date_Debut { get; set; }
        public DateTime Date_Fin { get; set; }
        public string Description { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise entreprise { get; set; }
        public int EtudiantId{ get; set; }
        public Etudiant Etudiant { get; set; }
        public Stage() { }
        public Stage(AddStageViewModel amv)
        {
            this.Intitulé_poste = amv.Intitulé_poste;
            this.Description = amv.Description;
            this.Date_Debut = amv.Date_Debut;
            this.Date_Fin = amv.Date_Fin;
            
        }

    }
}
