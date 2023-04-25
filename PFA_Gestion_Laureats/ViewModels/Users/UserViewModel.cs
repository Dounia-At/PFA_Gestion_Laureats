using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;


namespace PFA_Gestion_Laureats.ViewModels.Users
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Login), IsUnique = true)]
    public class UserViewModel
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Tel { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Titre")]
        public string? Titre_Profil { get; set; }
        public string? Adresse { get; set; }
        public string? Login { get; set; }

        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        public string? ConfirmationPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string? CurrentPassword { get; set; }
        public string? URL_Photo_Profil { get; set; }

        [ImageExtentionValidation(new string[] { ".png", ".jpg", ".jpeg" }, ErrorMessage = "l'extention doit être png, jpg ou jpeg")]
        public IFormFile? Photo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date d'inscription")]
        public DateTime? date_Inscription { get; set; }

        [Display(Name = "Specialité")]
        public string? specialite { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de fin d'étude")]
        public DateTime? Date_Fin_Etude { get; set; }

        public UserViewModel()
        {

        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel,
                            string Email, string Titre_Profil, string Adresse,
                            string password, string login, string path)
        {
            this.Id = Id;
            Nom = nom;
            Password = password;
            ConfirmationPassword = password;
            Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            Login = login;
            URL_Photo_Profil = path;
        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel,
                           string Email, string Titre_Profil, string Adresse,
                           string password, string login, string path,
                           string specialite, DateTime dateInscription)
        {
            this.Id = Id;
            Nom = nom;
            Password = password;
            ConfirmationPassword = password;
            Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            Login = login;
            URL_Photo_Profil = path;
            this.specialite = specialite;
            date_Inscription = dateInscription;
        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel,
                            string Email, string Titre_Profil, string Adresse,
                            string password, string login, string path,
                            string specialite, DateTime dateInscription, DateTime dateFinEtude)
        {
            this.Id = Id;
            Nom = nom;
            Password = password;
            ConfirmationPassword = password;
            Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            Login = login;
            URL_Photo_Profil = path;
            this.specialite = specialite;
            date_Inscription = dateInscription;
            Date_Fin_Etude = dateFinEtude;
        }

    }
}
