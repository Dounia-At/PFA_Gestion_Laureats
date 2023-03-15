namespace PFA_Gestion_Laureats.Models
{
    public class Formation
    {
        public int Id { get; set; }
        public DateTime Date_Debut { get; set; }
        public DateTime Date_Fin { get; set; }
        public string Diplome { get; set; }
        public string Ecole { get; set; }
        public string Description { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }


    }
}
