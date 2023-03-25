
﻿using Microsoft.AspNetCore.Mvc;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.ViewModels;

namespace PFA_Gestion_Laureats.Controllers
{
    public class UserController : Controller
    {
        MyContext db;
        public UserController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("Login", "asmaeJ");
            HttpContext.Session.SetString("Role", "Etudiant");
            return View();
        }

        public IActionResult InscriptionEtudiant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InscriptionEtudiant(UserViewModel user)
        {            
            if (ModelState.IsValid)
            {
                if(user.Nom != null && user.Prenom != null && user.Email != null
                    && user.Tel != null && user.Titre_Profil != null && user.Adresse != null
                    && user.date_Inscription !=null && user.Login != null && user.Password != null
                    && user.specialite != null)
                {
                    Etudiant etudiant = new Etudiant();
                    etudiant.Nom = user.Nom;
                    etudiant.Prenom = user.Prenom;
                    etudiant.Tel = user.Tel;
                    etudiant.Email = user.Email;
                    etudiant.Titre_Profil = user.Titre_Profil;
                    etudiant.Adresse = user.Adresse;
                    etudiant.Login = user.Login;
                    etudiant.Password = user.Password;
                    etudiant.date_Inscriptionion = (DateTime)user.date_Inscription;
                    etudiant.specialite = user.specialite;

                    db.Etudiants.Add(etudiant);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }                        

                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult InscriptionLaureat()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InscriptionLaureat(UserViewModel user)
        {            
            if (ModelState.IsValid)
            {
                if (user.Nom != null && user.Prenom != null && user.Email != null
                    && user.Tel != null && user.Titre_Profil != null && user.Adresse != null
                    && user.date_Inscription != null && user.Login != null && user.Password != null
                    && user.specialite != null && user.Date_Fin_Etude != null)
                {
                    Laureat laureat = new Laureat();
                laureat.Nom = user.Nom;
                laureat.Prenom = user.Prenom;
                laureat.Tel = user.Tel;
                laureat.Email = user.Email;
                laureat.Titre_Profil = user.Titre_Profil;
                laureat.Adresse = user.Adresse;
                laureat.Login = user.Login;
                laureat.Password = user.Password;
                laureat.date_Inscriptionion = (DateTime)user.date_Inscription;
                laureat.specialite = user.specialite;
                laureat.Date_Fin_Etude = (DateTime)user.Date_Fin_Etude;
                                            

                db.Laureats.Add(laureat);
                db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }

                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult UpdateCompte()
        {
           
            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                
                    UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom, 
                                                           utilisateur.Prenom, utilisateur.Tel,
                                                           utilisateur.Email, utilisateur.Titre_Profil,
                                                           utilisateur.Adresse, utilisateur.Password, 
                                                           utilisateur.Login, utilisateur.Photo_Profil);

                    return View(user);

            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateCompte(UserViewModel user)
        {
            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                if (ModelState.IsValid)
                {
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                   
                        utilisateur.Tel = user.Tel;
                        utilisateur.Email = user.Email;
                        utilisateur.Adresse = user.Adresse;
                        utilisateur.Titre_Profil = user.Titre_Profil;

                        db.Utilisateurs.Update(utilisateur);
                        db.SaveChanges();
                                                        
                    return RedirectToAction("Index");
                }
                return View(user);                

            }

            return RedirectToAction("Index");
            
        }

        public IActionResult UpdateSecurite()
        {

            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                           utilisateur.Prenom, utilisateur.Tel,
                                                           utilisateur.Email, utilisateur.Titre_Profil,
                                                           utilisateur.Adresse, utilisateur.Password,
                                                           utilisateur.Login, utilisateur.Photo_Profil);
                return View(user);               

            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateSecurite(UserViewModel user)
        {
            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                if (ModelState.IsValid && user.Password==user.ConfirmationPassword)
                {
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                    if(utilisateur.Password == user.CurrentPassword)
                    {
                        utilisateur.Password = user.Password;

                        db.Utilisateurs.Update(utilisateur);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.msgValidation = "Mot de passe actuel est eronne";
                        return View(user);
                    }
                   
                }
                else
                {
                    ViewBag.msgValidation = "Le Mot de passe et la confirmation sont different";
                    return View(user);
                }             

            }

            return RedirectToAction("Index");

        }
        public IActionResult UpdatePicture()
        {

            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                if(utilisateur.Photo_Profil == null)
                {
                    utilisateur.Photo_Profil = "1.png";
                }

                UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                       utilisateur.Prenom, utilisateur.Tel,
                                                       utilisateur.Email, utilisateur.Titre_Profil,
                                                       utilisateur.Adresse, utilisateur.Password,
                                                       utilisateur.Login, utilisateur.Photo_Profil);

                return View(user);

            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdatePicture(UserViewModel user)
        {
            if (user.Photo == null)
            {
                user.URL_Photo_Profil = "profil.png";
            }
            String login = HttpContext.Session.GetString("Login");
            if (login != null)
            {
                if (ModelState.IsValid)
                {
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();

                    string NewName = Guid.NewGuid() + user.Photo.FileName;
                    string PathFile = Path.Combine("wwwroot/assets/img/avatars", NewName);

                    using (FileStream stream = System.IO.File.Create(PathFile))
                    {
                        user.Photo.CopyTo(stream);
                        utilisateur.Photo_Profil = NewName;
                    }


                    db.Utilisateurs.Update(utilisateur);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(user);

            }

            return RedirectToAction("Index");

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel mv)
        {
            if(ModelState.IsValid)
            {
                Utilisateur utilisateur=db.Utilisateurs.Where(us=>us.Login==mv.Login &&us.Password==mv.Password ).FirstOrDefault();
                if (utilisateur != null)
                {
                    if (utilisateur.Isvalide == false)
                    {
                        ViewBag.msgValidation = "Ce compte n'est pas encore validé";
                    }
                    else
                    {
                        HttpContext.Session.SetString("login", utilisateur.Login);
                        HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                       
                        
                        if (utilisateur.GetType().Name == "AgentDirection")
                        {
                            HttpContext.Session.SetString("Role", "Agent");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                        }
                       
                        return RedirectToAction("Index", "Home");

                    }
                   
                    

                }
                else
                {
                    ViewBag.msgErreur = "Login ou Mot de passe incorrect";
                }
            }
           
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("login");
            return RedirectToAction("Login", "User");
        }
    }
}
