using PFA_Gestion_Laureats.Models;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels.Users
{
    public class ProfilViewModel : UserViewModel
    {

        public IList<Projet>? projets { get; set; }
        public IList<Stage>? stages { get; set; }
        public IList<Certificat>? certificats { get; set; }
        public IList<ExperiencePro>? experiences { get; set; }
        public IList<Formation>? formations { get; set; }
        public IList<Annonce>? annonces { get; set; }
        public ProfilViewModel() { }
        public ProfilViewModel(int Id, string nom, string prenom, string? Tel,
                           string Email, string Titre_Profil, string Adresse,
                           string password, string login, string path,
                           string specialite, DateTime dateInscription, IList<Projet>? projets,
                           IList<Stage>? stages, IList<Certificat>? certificats, IList<ExperiencePro>? experiences,
                           IList<Formation>? formations, IList<Annonce>? annonces) : base(Id, nom, prenom, Tel, Email, Titre_Profil, Adresse, password, login, path, specialite, dateInscription)
        {
            this.projets = projets;
            this.stages = stages;
            this.certificats = certificats;
            this.experiences = experiences;
            this.formations = formations;
            this.annonces = annonces;
        }
        public ProfilViewModel(Laureat l) : base(l.Id, l.Nom, l.Prenom, l.Tel, l.Email, l.Titre_Profil, l.Adresse, l.Password, l.Login, l.Photo_Profil, l.specialite, l.date_Inscriptionion, l.Date_Fin_Etude)
        {
            this.projets = l.projets;
            this.stages = l.stages;
            this.certificats = l.certificats;
            this. experiences = l.experiences;
            this. formations = l.formations;
            this.annonces = l.annonces;
        }
        public ProfilViewModel(Etudiant e) : base(e.Id, e.Nom, e.Prenom, e.Tel, e.Email, e.Titre_Profil, e.Adresse, e.Password, e.Login, e.Photo_Profil, e.specialite, e.date_Inscriptionion)
        {
            projets = e.projets;
            stages = e.stages;
            certificats = e.certificats;
            experiences = e.experiences;
            formations = e.formations;
        }
       
    }
}
