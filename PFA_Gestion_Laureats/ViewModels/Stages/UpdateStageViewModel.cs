using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.ViewModels.Stages
{
    public class UpdateStageViewModel : AddStageViewModel
    {
        public int Id { get; set; }
        public UpdateStageViewModel() { }
        public UpdateStageViewModel(Stage s)
        {
            this.Id = s.Id;
            this.Intitulé_poste = s.Intitulé_poste;
            this.Date_Debut = s.Date_Debut;
            this.Date_Fin = s.Date_Fin;
            this.Description = s.Description;
            this.Entreprise =s.entreprise.Nom;
        }
    }
}
