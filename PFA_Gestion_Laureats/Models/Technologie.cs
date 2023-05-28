namespace PFA_Gestion_Laureats.Models
{
    public class Technologie
    {
        public int Id { get; set; }       
        public string Libelle { get; set; }
        public string? Logo { get; set; }
        public IList<AnnonceTechnologie>? AnnonceTechnologies { get; set; }
    }
}
