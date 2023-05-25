using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.ViewModels.Messages
{
    public class MessageViewModel
    {
        public string contenu { get; set; }
        public DateTime Date_Envoie { get; set; }
        public bool status { get; set; }
        public Utilisateur UtilisateurSender { get; set; }
        public int SenderId { get; set; }

        public Utilisateur UtilisateurReceiver { get; set; }
        public int ReceiverId { get; set; }
    }
}
