
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
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
            return View();
        }
        [Route("/User/Detail/{login}")]
        [Authentification]
        public IActionResult Detail_User(string login)
        {
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).AsNoTracking().SingleOrDefault();
            if(utilisateur != null)
            {
                UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                        utilisateur.Prenom, utilisateur.Tel,
                                                        utilisateur.Email, utilisateur.Titre_Profil,
                                                        utilisateur.Adresse, null,
                                                        utilisateur.Login, utilisateur.Photo_Profil);

                if (utilisateur.GetType().Name == "AgentDirection")
                {
                    AgentDirection agent = db.Agents.Where(u => u.Login == login).AsNoTracking().SingleOrDefault();
                    user = new UserViewModel(agent.Id, agent.Nom,
                                                        agent.Prenom, agent.Tel,
                                                        agent.Email, agent.Titre_Profil,
                                                        agent.Adresse, null, agent.Login, agent.Photo_Profil);

                }
                else if (utilisateur.GetType().Name == "Etudiant")
                {
                    Etudiant etudiant = db.Etudiants.Where(u => u.Login == login).AsNoTracking().SingleOrDefault();
                    user = new UserViewModel(etudiant.Id, etudiant.Nom,
                                                        etudiant.Prenom, etudiant.Tel,
                                                        etudiant.Email, etudiant.Titre_Profil,
                                                        etudiant.Adresse, null, etudiant.Login, etudiant.Photo_Profil,
                                                        etudiant.specialite, etudiant.date_Inscriptionion);
                }
                else if (utilisateur.GetType().Name == "Laureat")
                {
                    Laureat laureat = db.Laureats.Where(u => u.Login == login).AsNoTracking().SingleOrDefault();
                    user = new UserViewModel(laureat.Id, laureat.Nom,
                                                        laureat.Prenom, laureat.Tel,
                                                        laureat.Email, laureat.Titre_Profil,
                                                        laureat.Adresse, null, laureat.Login, laureat.Photo_Profil,
                                                        laureat.specialite, laureat.date_Inscriptionion, laureat.Date_Fin_Etude);
                }

                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [IsAdmin]
        [Authentification]
        public IActionResult Add_Agent()
        {
            return View();
        }
        [IsAdmin]
        [Authentification]
        [HttpPost]
        public IActionResult Add_Agent(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Nom != null && user.Prenom != null && user.Email != null
                    && user.Tel != null && user.Titre_Profil != null && user.Adresse != null
                    && user.Login != null && user.Password != null)
                {
                    AgentDirection agent = new AgentDirection();
                    agent.Nom = user.Nom;
                    agent.Prenom = user.Prenom;
                    agent.Tel = user.Tel;
                    agent.Email = user.Email;
                    agent.Titre_Profil = user.Titre_Profil;
                    agent.Adresse = user.Adresse;
                    agent.Login = user.Login;
                    agent.Password = user.Password;
                    agent.Photo_Profil = "profil.png";
                    db.Agents.Add(agent);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }

                return RedirectToAction("Index", "Home");
            }
             return View(user);
        }


        [IsAdmin]
        [Authentification]
        public IActionResult Add_Etudiant()
        {
            return View();
        }
        [IsAdmin]
        [Authentification]
        [HttpPost]
        public IActionResult Add_Etudiant(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Nom != null && user.Prenom != null && user.Email != null
                    && user.Tel != null && user.Titre_Profil != null && user.Adresse != null
                    && user.date_Inscription != null && user.Login != null && user.Password != null
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
                    etudiant.Photo_Profil = "profil.png";

                    db.Etudiants.Add(etudiant);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(user);            
        }

        [IsAdmin]
        [Authentification]
        public IActionResult Add_Laureat()
        {
            return View();
        }
        [IsAdmin]
        [Authentification]
        [HttpPost]
        public IActionResult Add_Laureat(UserViewModel user)
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
                    laureat.Photo_Profil = "profil.png";

                    db.Laureats.Add(laureat);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoires!!";
                    return View(user);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [IsAdmin]
        [Authentification]
        [Route("/User/Update/{login}")]
        public IActionResult Update_User(string login)
        {
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).SingleOrDefault();
            if (utilisateur != null)
            {
                UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                        utilisateur.Prenom, utilisateur.Tel,
                                                        utilisateur.Email, utilisateur.Titre_Profil,
                                                        utilisateur.Adresse, null,
                                                        utilisateur.Login, null);

                if (utilisateur.GetType().Name == "AgentDirection")
                {
                    AgentDirection agent = db.Agents.Where(u => u.Login == login).SingleOrDefault();
                    user = new UserViewModel(agent.Id, agent.Nom,
                                                        agent.Prenom, agent.Tel,
                                                        agent.Email, agent.Titre_Profil,
                                                        agent.Adresse, null, agent.Login, null);

                }
                else if (utilisateur.GetType().Name == "Etudiant")
                {
                    Etudiant etudiant = db.Etudiants.Where(u => u.Login == login).SingleOrDefault();
                    user = new UserViewModel(etudiant.Id, etudiant.Nom,
                                                        etudiant.Prenom, etudiant.Tel,
                                                        etudiant.Email, etudiant.Titre_Profil,
                                                        etudiant.Adresse, null, etudiant.Login, null,
                                                        etudiant.specialite, etudiant.date_Inscriptionion);
                }
                else if (utilisateur.GetType().Name == "Laureat")
                {
                    Laureat laureat = db.Laureats.Where(u => u.Login == login).SingleOrDefault();
                    user = new UserViewModel(laureat.Id, laureat.Nom,
                                                        laureat.Prenom, laureat.Tel,
                                                        laureat.Email, laureat.Titre_Profil,
                                                        laureat.Adresse, null, laureat.Login, null,
                                                        laureat.specialite, laureat.date_Inscriptionion, laureat.Date_Fin_Etude);
                }

                return View(user);
            }
            return RedirectToAction("Index", "Home");

        }
        [IsAdmin]
        [Authentification]
        [HttpPost]
        [Route("/User/Update/{login}")]
        public IActionResult Update_User(string login, UserViewModel user)
        {
            if (ModelState.IsValid)
                {
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).SingleOrDefault();
                if (utilisateur != null)
                {

                    if (utilisateur.GetType().Name == "AgentDirection")
                    {
                        AgentDirection agent = db.Agents.Where(u => u.Login == login).SingleOrDefault();

                        agent.Nom = user.Nom;
                        agent.Prenom = user.Prenom;
                        agent.Tel = user.Tel;
                        agent.Email = user.Email;
                        agent.Titre_Profil = user.Titre_Profil;
                        agent.Adresse = user.Adresse;
                        agent.Login = user.Login;

                        db.Agents.Update(agent);

                    }
                    else if (utilisateur.GetType().Name == "Etudiant")
                    {
                        Etudiant etudiant = db.Etudiants.Where(u => u.Login == login).SingleOrDefault();

                        etudiant.Nom = user.Nom;
                        etudiant.Prenom = user.Prenom;
                        etudiant.Tel = user.Tel;
                        etudiant.Email = user.Email;
                        etudiant.Titre_Profil = user.Titre_Profil;
                        etudiant.Adresse = user.Adresse;
                        etudiant.Login = user.Login;
                        etudiant.date_Inscriptionion = (DateTime)user.date_Inscription;
                        etudiant.specialite = user.specialite;

                        db.Etudiants.Update(etudiant);
                    }
                    else if (utilisateur.GetType().Name == "Laureat")
                    {
                        Laureat laureat = db.Laureats.Where(u => u.Login == login).SingleOrDefault();

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

                        db.Laureats.Update(laureat);
                    }
                    db.SaveChanges();
                }                                 
                    return RedirectToAction("Index", "Home");
                }
                return View(user);
        }


       

        [IsAdmin]
        [Authentification]
        [Route("/User/Delete/{login}")]
        public IActionResult Delete_User(string login)
        {
             Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).SingleOrDefault();
            db.Utilisateurs.Remove(utilisateur);
                db.SaveChanges();
                       
            return RedirectToAction("Index", "Home");
        }
        

        public IActionResult Inscription_Etudiant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Inscription_Etudiant(UserViewModel user)
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
                    etudiant.Photo_Profil = "profil.png";

                    db.Etudiants.Add(etudiant);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }                        

                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Inscription_Laureat()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Inscription_Laureat(UserViewModel user)
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
                laureat.Photo_Profil = "profil.png";

                db.Laureats.Add(laureat);
                db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Les champs sont obligatoire!!";
                    return View(user);
                }

                return RedirectToAction("Login");
            }
            return View(user);
        }

        [Authentification]
        public IActionResult Update_Compte()
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                
            UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom, 
                                                    utilisateur.Prenom, utilisateur.Tel,
                                                    utilisateur.Email, utilisateur.Titre_Profil,
                                                    utilisateur.Adresse, utilisateur.Password, 
                                                    utilisateur.Login, utilisateur.Photo_Profil);

            return View(user);
        }
        [Authentification]
        [HttpPost]
        public IActionResult Update_Compte(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                String login = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                   
                    utilisateur.Tel = user.Tel;
                    utilisateur.Email = user.Email;
                    utilisateur.Adresse = user.Adresse;
                    utilisateur.Titre_Profil = user.Titre_Profil;

                    db.Utilisateurs.Update(utilisateur);
                    db.SaveChanges();
                                                        
                return RedirectToAction("Index", "Home");
            }
            return View(user);            
            
        }

        [Authentification]
        public IActionResult Update_Securite()
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
            UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                        utilisateur.Prenom, utilisateur.Tel,
                                                        utilisateur.Email, utilisateur.Titre_Profil,
                                                        utilisateur.Adresse, utilisateur.Password,
                                                        utilisateur.Login, utilisateur.Photo_Profil);
            return View(user);               
        }
        [Authentification]
        [HttpPost]
        public IActionResult Update_Securite(UserViewModel user)
        {            
            if (ModelState.IsValid )
            {
                if(user.Password == user.ConfirmationPassword)
                {
                    String login = HttpContext.Session.GetString("Login");
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                    if (utilisateur.Password == user.CurrentPassword)
                    {
                        utilisateur.Password = user.Password;

                        db.Utilisateurs.Update(utilisateur);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
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
            return View(user);

        }

        [Authentification]
        public IActionResult Update_Picture()
        {

            String login = HttpContext.Session.GetString("Login");
            
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
            if(utilisateur.Photo_Profil == null)
            {
                utilisateur.Photo_Profil = "profil.png";
            }

            UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                    utilisateur.Prenom, utilisateur.Tel,
                                                    utilisateur.Email, utilisateur.Titre_Profil,
                                                    utilisateur.Adresse, utilisateur.Password,
                                                    utilisateur.Login, utilisateur.Photo_Profil);
            return View(user);
        }
        [Authentification]
        [HttpPost]
        public IActionResult Update_Picture(UserViewModel user)
        {
            if (user.Photo == null)
            {
                user.URL_Photo_Profil = "profil.png";
            }
            String login = HttpContext.Session.GetString("Login");
           
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                System.IO.File.Delete(Path.Combine("wwwroot/assets/img/avatars", utilisateur.Photo_Profil));
                string NewName = Guid.NewGuid() + user.Photo.FileName;
                string PathFile = Path.Combine("wwwroot/assets/img/avatars", NewName);

                using (FileStream stream = System.IO.File.Create(PathFile))
                {
                    user.Photo.CopyTo(stream);
                    utilisateur.Photo_Profil = NewName;
                }
                

                db.Utilisateurs.Update(utilisateur);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(user);

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
                        HttpContext.Session.SetString("Login", utilisateur.Login);
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
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login", "User");
        }
    }
}
