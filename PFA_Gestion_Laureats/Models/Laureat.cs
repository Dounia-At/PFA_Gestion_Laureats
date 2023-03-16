using PFA_Gestion_Laureats.Services;

namespace PFA_Gestion_Laureats.Models
{
    public class Laureat: Etudiant, IModirateur
    {
        public DateTime Date_Fin_Etude { get; set; }
    }
}
