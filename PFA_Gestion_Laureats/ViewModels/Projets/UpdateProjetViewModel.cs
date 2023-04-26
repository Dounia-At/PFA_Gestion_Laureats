using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.ViewModels.Projets
{
    public class UpdateProjetViewModel : AddProjetViewModel
    {
        public int Id { get; set; }
        public UpdateProjetViewModel() { }
        public UpdateProjetViewModel(Projet p)
        {
            Id = p.Id;
            Nom = p.Nom;
            Date_Fin = p.Date_Fin;
            Date_Debut = p.Date_Debut;
            Description = p.Description;
        }
    }
}
