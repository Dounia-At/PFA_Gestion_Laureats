
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using System.Net.Mail;
using System.Net;
using PFA_Gestion_Laureats.ViewModels.Users;
using System.Text;
using NuGet.Protocol.Plugins;

namespace PFA_Gestion_Laureats.Controllers
{
    public class UserController : Controller
    {
        MyContext db;
        public UserController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Confirm(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            utilisateur.IsComfirmed = true;
            db.Utilisateurs.Update(utilisateur);
            db.SaveChanges();

            return RedirectToAction("Login");
        }
        [Authentification]
        public IActionResult Validation()
        {            
            List<Utilisateur> utilisateurs = db.Utilisateurs.Where(u => u.Isvalide == false && u.IsComfirmed == true).AsNoTracking().ToList();
            return View(utilisateurs);
        }
        [Route("/User/Valider/{login}")]
        [Authentification]
        public IActionResult Valider(string login)
        {
            
            try
            {
                String loginAgent = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).SingleOrDefault();
                AgentDirection agent = db.Agents.Where(u => u.Login == loginAgent).SingleOrDefault();

                using (MailMessage mail = new MailMessage(agent.Email, utilisateur.Email))
                {

                    mail.Subject = "Votre compte est valide!!";
                    // href!!!
                    mail.Body = "<p>L'ecole EHEI vous informe que votre compte a été valider.</p>" +
                                "<a class=\"btn btn-primary\" href='User/Login/"+utilisateur+"' > Bienvenure! </a>";
                    mail.BodyEncoding = Encoding.UTF8;

                    mail.IsBodyHtml = true;
                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";

                        NetworkCredential NetworkCred = new NetworkCredential(agent.Email, utilisateur.Password);
                        // smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;

                        smtp.Port = 587;
                        //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                        smtp.Send(mail);

                    }

                }
                utilisateur.Isvalide = true;
                db.Utilisateurs.Update(utilisateur);
                db.SaveChanges();
            }
            catch (System.Net.Mail.SmtpException ex)
            {

                ViewBag.Alert = "Ressayer ultérieurement!";
                return View();


            }
            return RedirectToAction("Validation");
        }
        [Authentification]
        public async Task< List<Utilisateur> > ImportExcel(IFormFile file)
        {
            var list = new List<Utilisateur>();
            using(var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for(int row = 2; row <+ rowcount; row++)
                    {
                        list.Add(new Utilisateur
                        {
                            Nom = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            // ....
                        });
                    }
                }
            }
            return list;
        }


        [Route("/User/Details/{login}")]
        [Authentification]
        public IActionResult Detail_User(string login)
        {
            Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).AsNoTracking().SingleOrDefault();
            
            if (utilisateur != null)
            {
                UserViewModel user = new UserViewModel(utilisateur.Id, utilisateur.Nom,
                                                        utilisateur.Prenom, utilisateur.Tel,
                                                        utilisateur.Email, utilisateur.Titre_Profil,
                                                        utilisateur.Adresse, null,
                                                        utilisateur.Login, utilisateur.Photo_Profil);

                if (utilisateur.GetType().Name == "AgentDirection")
                {
                    AgentDirection agent = db.Agents.Where(u => u.Login == login).Include(e => e.annonces).AsNoTracking().SingleOrDefault();
                    user = new UserViewModel(agent.Id,agent.Nom,agent.Prenom,agent.Tel,agent.Email,agent.Titre_Profil,agent.Adresse,agent.Password,agent.Login,agent.Photo_Profil);
                    return View("Profil",user);

                }
                else if (utilisateur.GetType().Name == "Etudiant")
                {
                    Etudiant etudiant = db.Etudiants.Where(u => u.Login == login).Include(e=>e.projets).Include(e => e.stages).Include(e => e.experiences).Include(e => e.formations).Include(e => e.certificats).AsNoTracking().SingleOrDefault();
                   
                    user = new ProfilViewModel(etudiant);
                }
                else if (utilisateur.GetType().Name == "Laureat")
                {
                    Laureat laureat = db.Laureats.Where(u => u.Login == login).Include(L => L.projets).Include(e => e.stages).ThenInclude(e=>e.entreprise).Include(e => e.experiences).ThenInclude(e => e.entreprise).Include(e => e.formations).Include(e => e.certificats).Include(e => e.certificats).Include(e=>e.annonces).ThenInclude(e=>e.entreprise).AsNoTracking().SingleOrDefault();

                    user = new ProfilViewModel(laureat);
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
                if (!db.Utilisateurs.Any(u => u.Login == user.Login))
                {
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login deja existant!!";
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
                if (!db.Utilisateurs.Any(u => u.Login == user.Login))
                {
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login deja existant!!";
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
                if (!db.Utilisateurs.Any(u => u.Login == user.Login))
                {
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login deja existant!!";
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
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }
                    return View(user);
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

            return RedirectToAction("Validation");
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
                if (!db.Utilisateurs.Any(u => u.Login == user.Login))
                {
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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

                            try
                            {
                                String loginAgent = HttpContext.Session.GetString("Login");
                                AgentDirection agent = db.Agents.Where(u => u.Login == loginAgent).SingleOrDefault();

                                using (MailMessage mail = new MailMessage(agent.Email, etudiant.Email))
                                {

                                    mail.Subject = "Confirmer votre adresse!!";
                                    // href!!!
                                    mail.Body = "<p>Veuillez cliquer sur le bouton suivant pour confirmer votre inscription.</p>" +
                                                "<a class=\"btn btn-primary\" href='User/Confirm/" + etudiant.Id + "' > Confirmation d'adresse! </a>";
                                    mail.BodyEncoding = Encoding.UTF8;

                                    mail.IsBodyHtml = true;
                                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                                    {
                                        smtp.Host = "smtp.gmail.com";

                                        NetworkCredential NetworkCred = new NetworkCredential(agent.Email, etudiant.Password);
                                        // smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                                        smtp.EnableSsl = true;
                                        smtp.UseDefaultCredentials = false;
                                        smtp.Credentials = NetworkCred;

                                        smtp.Port = 587;
                                        //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                                        smtp.Send(mail);

                                    }

                                }
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {

                                ViewBag.Alert = "Une erreur est survenue, veuillez réessayer ultérieurement!";
                                return View();


                            }

                            db.Etudiants.Add(etudiant);
                            db.SaveChanges();

                        }
                        else
                        {
                            ViewBag.msgValidation = "Les champs sont obligatoire!!";
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login deja existant!!";
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
                if (!db.Utilisateurs.Any(u => u.Login == user.Login))
                {
                    if (!db.Utilisateurs.Any(u => u.Email == user.Email))
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

                            try
                            {
                                String loginAgent = HttpContext.Session.GetString("Login");
                                AgentDirection agent = db.Agents.Where(u => u.Login == loginAgent).SingleOrDefault();

                                using (MailMessage mail = new MailMessage(agent.Email, laureat.Email))
                                {

                                    mail.Subject = "Confirmer votre adresse!!";
                                    // href!!!
                                    mail.Body = "<p>Veuillez cliquer sur le bouton suivant pour confirmer votre inscription.</p>" +
                                                "<a class=\"btn btn-primary\" href='User/Confirm/" + laureat.Id + "' > Confirmation d'adresse! </a>";
                                    mail.BodyEncoding = Encoding.UTF8;

                                    mail.IsBodyHtml = true;
                                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                                    {
                                        smtp.Host = "smtp.gmail.com";

                                        NetworkCredential NetworkCred = new NetworkCredential(agent.Email, laureat.Password);
                                        // smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                                        smtp.EnableSsl = true;
                                        smtp.UseDefaultCredentials = false;
                                        smtp.Credentials = NetworkCred;

                                        smtp.Port = 587;
                                        //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                                        smtp.Send(mail);

                                    }

                                }
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {

                                ViewBag.Alert = "Une erreur est survenue, veuillez réessayer ultérieurement!";
                                return View();


                            }

                            db.Laureats.Add(laureat);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.msgValidation = "Les champs sont obligatoire!!";
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login deja existant!!";
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
                if (!db.Utilisateurs.Any(u => u.Email == user.Email))
                {
                    String login = HttpContext.Session.GetString("Login");
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();

                    utilisateur.Tel = user.Tel;
                    utilisateur.Email = user.Email;
                    utilisateur.Adresse = user.Adresse;
                    utilisateur.Titre_Profil = user.Titre_Profil;

                    db.Utilisateurs.Update(utilisateur);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.msgValidation = "Email deja existant!!";
                    return View(user);
                }
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
            if (ModelState.IsValid)
            {
                if (user.Password == user.ConfirmationPassword)
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
            if (utilisateur.Photo_Profil == null)
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
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == mv.Login && us.Password == mv.Password).FirstOrDefault();
                if (utilisateur != null)
                {
                    if(!utilisateur.IsComfirmed)
                    {
                        ViewBag.msgValidation = "Ce compte n'est pas encore Confirmé!";
                        return View();
                    }
                    else if (utilisateur.Isvalide == false)
                    {
                        ViewBag.msgValidation = "Ce compte n'est pas encore validé!";
                        return View();
                    }
                    else
                    {
                        HttpContext.Session.SetString("Login", utilisateur.Login);
                        HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                        HttpContext.Session.SetString("photo", utilisateur.Photo_Profil);
                      
                        utilisateur.date_Login= DateTime.Now;
                        db.Utilisateurs.Update(utilisateur);
                        db.SaveChanges();
                        return RedirectToAction("Annonces", "Annonce");

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
            string login=HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();

            utilisateur.date_Logout = DateTime.Now;
            db.Utilisateurs.Update(utilisateur);
            db.SaveChanges();
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login", "User");
        }
    }
}
