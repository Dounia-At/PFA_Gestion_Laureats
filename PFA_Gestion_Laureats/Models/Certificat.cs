namespace PFA_Gestion_Laureats.Models
{
    public class Certificat
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Organisation { get; set; }
        public DateTime Date_Emission { get; set; }
        public DateTime Date_Expiration { get; set; }
        public string? Url { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }
    }
}
