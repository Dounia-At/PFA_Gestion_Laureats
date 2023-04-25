using PFA_Gestion_Laureats.Models;


namespace PFA_Gestion_Laureats.ViewModels.ExperiencePros
{
    public class UpdateExperienceProViewModel:AddExperienceProViewModel
    {
        public int Id { get; set; }
        public UpdateExperienceProViewModel() { }
        public UpdateExperienceProViewModel(ExperiencePro experiencePro)
        {
            this.Id = experiencePro.Id;
            this.Post = experiencePro.Post;
            this.Type_Emploi = experiencePro.Type_Emploi;
            this.Etat = experiencePro.Etat;
            this.Entreprise = experiencePro.entreprise.Nom;
          

        }
    }
}
