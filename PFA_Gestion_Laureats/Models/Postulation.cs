using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
 {
    public class Postulation
    {
        public int Id { get; set; }

        public DateTime? Date_Postulation { get; set; }
        public DateTime Date_Consultation { get; set; }
        
        public Etudiant Etudiant { get; set; }
        public int EtudiantId { get; set; }
        public Annonce Annonce { get; set; }
        public int AnnonceId { get; set; }
    }
}
