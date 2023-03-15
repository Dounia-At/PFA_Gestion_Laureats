using PFA_Gestion_Laureats.Services;

namespace PFA_Gestion_Laureats.Models
{
    public class AgentDirection : Utilisateur, IModirateur
    {
        public IList<Annonce>? annonces { get; set; }
        public IList<Test>? tests { get; set; }
    }
}
