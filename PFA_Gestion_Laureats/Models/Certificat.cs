using PFA_Gestion_Laureats.ViewModels.Certificats;

namespace PFA_Gestion_Laureats.Models
{
    public class Certificat
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Organisation { get; set; }
        public DateTime Date_Emission { get; set; }
        public DateTime ?Date_Expiration { get; set; }
        public string? Url { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }
        public Certificat() { }
        public Certificat(AddCertificatViewModel amv)
        {
            this.Nom = amv.Nom;
            this.Organisation = amv.Organisation;
            this.Date_Expiration = amv.Date_Expiration;
            this.Date_Emission = amv.Date_Emission;
            this.Url = amv.Url;
            this.EtudiantId = amv.EtudiantId;
        }
    }
}
