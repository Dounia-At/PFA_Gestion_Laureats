﻿using PFA_Gestion_Laureats.Validation;
using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.ViewModels
{
    public class UserViewModel
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Tel { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Titre_Profil { get; set; }
        public string? Adresse { get; set; }
        public string? Login { get; set; }

        
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [DataType(DataType.Password)]
        public string? ConfirmationPassword { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        public string? URL_Photo_Profil { get; set; }

       [ImageExtentionValidation(new string[] { ".png", ".jpg", ".jpeg" }, ErrorMessage = "l'extention doit être png, jpg ou jpeg")]
       public IFormFile? Photo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? date_Inscription { get; set; }

        public string? specialite { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_Fin_Etude { get; set; }

        public UserViewModel()
        {

        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel,
                            string Email, string Titre_Profil, string Adresse,
                            string password, string login, string path )
        {
            this.Id = Id;
            this.Nom = nom;
            this.Password = password;
            this.ConfirmationPassword = password;
            this.Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            this.Login = login;
            this.URL_Photo_Profil = path;
        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel,
                             string Email, string Titre_Profil, string Adresse,
                             string password, string login, string path,
                             string specialite, DateTime dateInscription)
        {
            this.Id = Id;
            this.Nom = nom;
            this.Password = password;
            this.ConfirmationPassword = password;
            this.Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            this.Login = login;
            this.URL_Photo_Profil = path;
            this.specialite = specialite;
            this.date_Inscription = dateInscription;
        }
        public UserViewModel(int Id, string nom, string prenom, string? Tel, 
                            string Email, string Titre_Profil, string Adresse, 
                            string password, string login, string path,
                            string specialite, DateTime dateInscription, DateTime dateFinEtude)
        {
            this.Id = Id;
            this.Nom = nom;
            this.Password = password;
            this.ConfirmationPassword = password;
            this.Prenom = prenom;
            this.Tel = Tel;
            this.Email = Email;
            this.Titre_Profil = Titre_Profil;
            this.Adresse = Adresse;
            this.Login = login;
            this.URL_Photo_Profil = path;
            this.specialite = specialite;
            this.date_Inscription = dateInscription;
            this.Date_Fin_Etude = dateFinEtude;
        }
    }
}