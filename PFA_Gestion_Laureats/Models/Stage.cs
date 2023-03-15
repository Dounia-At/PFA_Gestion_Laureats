namespace PFA_Gestion_Laureats.Models
{
    public class Stage
    {
        public int Id { get; set; }
        public string Sujet { get; set; }
        public DateTime Date_Debut { get; set; }
        public DateTime Date_Fin { get; set; }
        public string Description { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise entreprise { get; set; }
        public int EtudiantId{ get; set; }
        public Etudiant Etudiant { get; set; }

    }
}
