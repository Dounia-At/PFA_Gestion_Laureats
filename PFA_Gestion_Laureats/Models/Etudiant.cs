namespace PFA_Gestion_Laureats.Models
{
    public class Etudiant:Utilisateur
    {
        public DateTime date_Inscriptionion { get; set; }
        public string specialite { get; set; }
        public IList<Projet> ? projets { get; set; }
        public IList<Stage> ? stages { get; set; }
        public IList<Certificat> ? certificats { get; set; }
        public IList<ExperiencePro> ? experiences { get; set; }
        public IList<Formation> ? formations { get; set; }
        public IList<Postulation> ? postulations { get; set; }

    }
}
