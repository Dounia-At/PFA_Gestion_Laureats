namespace PFA_Gestion_Laureats.Models
{
    public class Technologie
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public IList<Annonce>? annonces { get; set; }
    }
}
