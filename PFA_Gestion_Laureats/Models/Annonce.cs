using PFA_Gestion_Laureats.Services;

namespace PFA_Gestion_Laureats.Models
{
    public class Annonce
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public string Email_Reception { get; set; }
        public DateTime Date_limite_Deposer { get; set; }
        public DateTime Date_Creation { get; set; }

        public IList<Postulation>? postulations { get; set; }

        public IModirateur Modirateur { get; set; }
        
        

    }
}
