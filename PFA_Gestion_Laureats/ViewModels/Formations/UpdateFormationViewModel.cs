using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.ViewModels.Formations
{
    public class UpdateFormationViewModel : AddFormationViewModel
    {
        public int Id { get; set; }

        public UpdateFormationViewModel() { }
        public UpdateFormationViewModel(Formation f)
        {
            Id = f.Id;
            Ecole = f.Ecole;
            Date_Fin = f.Date_Fin;
            Date_Debut = f.Date_Debut;
            Description = f.Description;
            Diplome = f.Diplome;
        }
    }
}
