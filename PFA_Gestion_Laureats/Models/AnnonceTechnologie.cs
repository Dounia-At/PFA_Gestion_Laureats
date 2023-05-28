namespace PFA_Gestion_Laureats.Models
{
    public class AnnonceTechnologie
    {
        public int AnnonceId { get; set; }
        public Annonce Annonce { get; set; }
        public int TechnologieId { get; set; }
        public Technologie Technologie { get; set; }

    }
}
