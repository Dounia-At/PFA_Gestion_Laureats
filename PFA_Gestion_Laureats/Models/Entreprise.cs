namespace PFA_Gestion_Laureats.Models
{
    public class Entreprise
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Pays { get; set; }
        public string Ville { get; set; }
        public string Adresse { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool Convention { get; set; }
        public IList<Test>? tests { get; set; }
        public IList<ExperiencePro>? experiences { get; set; }
        public IList<Stage>? stages { get; set; }
        public IList<Annonce>? annonces { get; set; }

    }
}
