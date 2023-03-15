using PFA_Gestion_Laureats.Services;

namespace PFA_Gestion_Laureats.Models
{
    public class Laureat
    {
        public DateTime Date_Fin_Etude { get; set; }
        public IList<Annonce>? annonces { get; set; }
    }
}
