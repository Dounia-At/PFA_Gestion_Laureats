namespace PFA_Gestion_Laureats.Models
{
    public class ExperiencePro
    {
        public int Id { get; set; }
        public string Post { get; set; }
        public string Type_Emploi { get; set; }
        public bool Etat { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise entreprise { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }


    }
}
