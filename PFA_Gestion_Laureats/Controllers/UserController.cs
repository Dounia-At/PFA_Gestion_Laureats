
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
using Microsoft.AspNetCore.Http;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using System.Web.Helpers;

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
            //string senderEmail = "abdellaouihajar826@gmail.com"; // Adresse e-mail de l'expéditeur
            //string senderPassword = "Sjihoqhsnsvlnpvn"; // Mot de passe de l'expéditeur

            //MailMessage mail = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            //// Remplir les détails de l'e-mail
            //mail.From = new MailAddress(senderEmail);
            //mail.To.Add(utilisateur.Email);
            //mail.Subject = "Validation de votre compte";
            //// href!!!
           

            //// Envoyez l'email de confirmation
            //mail.Body = $"Veuillez attendre que l'administration valide votre compte. Merci ";

           
            //mail.BodyEncoding = Encoding.UTF8;

            //mail.IsBodyHtml = true;

            //// Configurer le client SMTP
            //smtpClient.EnableSsl = true;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            ViewBag.Confirmation = "Veuillez verifier votre compte gmail pour la confirmation ";
            // Envoyer l'e-mails
            //smtpClient.Send(mail);

            //return Redirect("https://mail.google.com/mail/u/1/?ogbl#inbox");
            TempData["Alert"] = "Veuillez attendre que l'administration valide votre compte.";

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
                string senderEmail = "abdellaouihajar826@gmail.com"; // Adresse e-mail de l'expéditeur
                string senderPassword = "Sjihoqhsnsvlnpvn"; // Mot de passe de l'expéditeur

                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); 

                // Remplir les détails de l'e-mail
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(utilisateur.Email);
                mail.Subject = "Votre compte est valide!!";
                mail.Body = "<p>L'Ecole EHEI vous informe que votre compte a été valider.</p>";
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;

                // Configurer le client SMTP
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Envoyer l'e-mail
                smtpClient.Send(mail);

               
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
        [IsAdmin]
        public IActionResult ImportUsers()
        {
            return View();
        }
        [IsAdmin]
        [HttpPost]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                ModelState.AddModelError("", "Please select a CSV file to upload.");
                return View();
            }

            var data = new MemoryStream();
            await file.CopyToAsync(data);

            data.Position = 0;
            using (var reader = new StreamReader(data))
            {
                var bad = new List<string>();
                var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";",
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    BadDataFound = context =>
                    {
                        bad.Add(context.RawRecord);
                    }
                };
                using (var csvReader = new CsvReader(reader, conf))
                {
                    while (csvReader.Read())
                    {
                        var NomComplet = csvReader.GetField(0).ToString().TrimEnd();
                        var fullNameParts = NomComplet.Split(' ');
                        string Prn, Name, field, phone = "";
                        if (fullNameParts[0].Length < 3)
                        {
                            Prn = fullNameParts[fullNameParts.Length - 1];
                            Name = string.Join(" ", fullNameParts.Take(fullNameParts.Length - 1));                            
                        }
                        else
                        {
                            Name = fullNameParts[0];
                            Prn = string.Join(" ", fullNameParts.Skip(1));
                        }
                        string firstInitial = Name.ToUpper();
                        string firstName = Prn?.Replace(" ", "") ?? string.Empty;
                        string baseUsername = firstInitial.Replace(" ", "");

                        if (!string.IsNullOrEmpty(firstName) ) {
                            firstInitial = Name.Substring(0, 2).ToUpper();
                            baseUsername = firstInitial + char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower();
                        }
                        
                        string username = baseUsername;
                        int suffix = 1;

                        // Check if the generated username already exists
                        while (db.Utilisateurs.Any(u => u.Login == username))
                        {
                            // Append a numeric suffix to the base username
                            username = baseUsername + suffix;
                            suffix++;
                        }


                        var Adr = csvReader.GetField(2).ToString();
                        if (csvReader.GetField(1).ToString() != "" && csvReader.GetField(1).ToString() != null)
                        {
                            phone = "0" + csvReader.GetField(1).ToString();
                        }
                        var titre = csvReader.GetField(5).ToString();

                        if (titre.Contains("Cycle préparatoire intégré"))
                        {
                            field = "CPI";
                        }
                        else if (titre.Contains("Génie des systèmes industriels"))
                        {
                            field = "GSI";
                        }
                        else if (titre.Contains("Génie informatique"))
                        {
                            field = "GI";
                        }
                        else
                        {
                            field = "IG";
                        }

                        var date = csvReader.GetField(4).ToString();

                        DateTime dateInsc;
                        if (!DateTime.TryParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateInsc)
                            && !DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateInsc))
                        {
                            // Invalid date format, use a fixed date as fallback
                            dateInsc = new DateTime(DateTime.Now.Year, 10, 1); // Use any valid date you prefer
                        }

                        string pass = Guid.NewGuid().ToString("N").Substring(0, 8);

                        await db.Etudiants.AddAsync(new Etudiant
                        {
                            Nom = Name,
                            Prenom = Prn,
                            Adresse = Adr ?? "",
                            Tel = phone ?? "",
                            date_Inscriptionion = dateInsc,
                            specialite = field,
                            Titre_Profil = titre,
                            Login = username,
                            Email = null,
                            Password = pass,
                            Isvalide = true,
                            IsComfirmed = true,
                            Photo_Profil = "profil.png"
                        });
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
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
                    AgentDirection agent = db.Agents.Where(u => u.Login == login).Include(e => e.annonces).ThenInclude(e => e.entreprise).AsNoTracking().SingleOrDefault();
                    user = new UserViewModel(agent.Id,agent.Nom,agent.Prenom,agent.Tel,agent.Email,agent.Titre_Profil,agent.Adresse,agent.Password,agent.Login,agent.Photo_Profil);
                    user.annonces = agent.annonces;
                    return View("Profil",user);

                }
                else if (utilisateur.GetType().Name == "Etudiant")
                {
                    Etudiant etudiant = db.Etudiants.Where(u => u.Login == login)
                        .Include(e=>e.projets.OrderByDescending(p => p.Date_Fin).Take(3))
                        .Include(e => e.stages.OrderByDescending(s => s.Date_Fin).Take(3)).ThenInclude(e => e.entreprise)
                        .Include(e => e.experiences.OrderByDescending(p => p.Id).Take(3)).ThenInclude(e => e.entreprise)
                        .Include(e => e.formations.OrderByDescending(f => f.Date_Fin).Take(3))
                        .Include(e => e.certificats.OrderByDescending(c => c.Date_Emission).Take(3)).AsNoTracking().SingleOrDefault();
                   
                    user = new ProfilViewModel(etudiant);
                }
                else if (utilisateur.GetType().Name == "Laureat")
                {
                    Laureat laureat = db.Laureats.Where(u => u.Login == login)
                        .Include(L => L.projets.OrderByDescending(p => p.Date_Fin).Take(3))
                        .Include(e => e.stages.OrderByDescending(p => p.Date_Fin).Take(3))
                        .ThenInclude(e=>e.entreprise).Include(e => e.experiences.OrderByDescending(p => p.Id).Take(3))
                        .ThenInclude(e => e.entreprise).Include(e => e.formations.OrderByDescending(p => p.Date_Fin).Take(3))
                        .Include(e => e.certificats.OrderByDescending(p => p.Date_Emission).Take(3))
                        .Include(e=>e.annonces).ThenInclude(e=>e.entreprise).AsNoTracking().SingleOrDefault();

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
                            db.Etudiants.Add(etudiant);
                            db.SaveChanges();
                            Utilisateur utilisateur = db.Utilisateurs.Where(etu=>etu.Login==etudiant.Login).FirstOrDefault();

                            try
                            {
                                String loginAgent = HttpContext.Session.GetString("Login");
                                AgentDirection agent = db.Agents.Where(u => u.Login == loginAgent).SingleOrDefault();
                                string senderEmail = "abdellaouihajar826@gmail.com"; // Adresse e-mail de l'expéditeur
                                string senderPassword = "Sjihoqhsnsvlnpvn"; // Mot de passe de l'expéditeur

                                MailMessage mail = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                                // Remplir les détails de l'e-mail
                                mail.From = new MailAddress(senderEmail);
                                mail.To.Add(etudiant.Email);
                                mail.Subject = "Confirmer votre adresse!!";
                                // href!!!
                                var confirmationLink = Url.Action("Confirm", "User", new { Id = utilisateur.Id },HttpContext.Request.Scheme );

                                // Envoyez l'email de confirmation
                                mail.Body = $"Veuillez cliquer sur le lien suivant pour confirmer votre email : <a href='{confirmationLink}'>Confirmation Email</a>";
                               
                                //mail.Body = "<p>Veuillez cliquer sur le bouton suivant pour confirmer votre inscription.</p>" +
                                //            "<a class=\"btn btn-primary\"  asp-action=\"Confirm\"" + etudiant.Id + "' > Confirmation d'adresse! </a>";
                                mail.BodyEncoding = Encoding.UTF8;

                                mail.IsBodyHtml = true;

                                // Configurer le client SMTP
                                smtpClient.EnableSsl = true;
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                                // Envoyer l'e-mail
                                smtpClient.Send(mail);

                                //using (MailMessage mail = new MailMessage(agent.Email, etudiant.Email))
                                //{

                                //    mail.Subject = "Confirmer votre adresse!!";
                                //    // href!!!
                                //    mail.Body = "<p>Veuillez cliquer sur le bouton suivant pour confirmer votre inscription.</p>" +
                                //                "<a class=\"btn btn-primary\" href='User/Confirm/" + etudiant.Id + "' > Confirmation d'adresse! </a>";
                                //    mail.BodyEncoding = Encoding.UTF8;

                                //    mail.IsBodyHtml = true;
                                //    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                                //    {
                                //        smtp.Host = "smtp.gmail.com";

                                //        NetworkCredential NetworkCred = new NetworkCredential(agent.Email, etudiant.Password);
                                //        // smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                                //        smtp.EnableSsl = true;
                                //        smtp.UseDefaultCredentials = false;
                                //        smtp.Credentials = NetworkCred;

                                //        smtp.Port = 587;
                                //        //smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                                //        smtp.Send(mail);

                                //    }

                                //}
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {

                                ViewBag.Alert = "Une erreur est survenue, veuillez réessayer ultérieurement!";
                                return View();


                            }

                            

                        }
                        else
                        {
                            ViewBag.msgValidation = "Les champs sont obligatoire!!";
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.msgValidation = "Email déja existe!!";
                        return View(user);
                    }

                }
                else
                {
                    ViewBag.msgValidation = "Login déja existe!!";
                    return View(user);
                }
                ViewBag.Confirmation = "Veuillez verifier votre compte gmail pour la confirmation ";
                //return RedirectToAction("Login");
                return View(user);
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
                            db.Laureats.Add(laureat);
                            db.SaveChanges();
                            Utilisateur utilisateur = db.Utilisateurs.Where(etu => etu.Login == laureat.Login).FirstOrDefault();
                            try
                            {
                                String loginAgent = HttpContext.Session.GetString("Login");
                                AgentDirection agent = db.Agents.Where(u => u.Login == loginAgent).SingleOrDefault();
                               
                                string senderEmail = "abdellaouihajar826@gmail.com"; // Adresse e-mail de l'expéditeur
                                string senderPassword = "Sjihoqhsnsvlnpvn"; // Mot de passe de l'expéditeur

                                MailMessage mail = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                                // Remplir les détails de l'e-mail
                                mail.From = new MailAddress(senderEmail);
                                mail.To.Add(laureat.Email);
                                mail.Subject = "Confirmer votre adresse!!";
                                // href!!!
                                var confirmationLink = Url.Action("Confirm", "User", new { Id = utilisateur.Id }, HttpContext.Request.Scheme);

                                // Envoyez l'email de confirmation
                                mail.Body = $"Veuillez cliquer sur le lien suivant pour confirmer votre email : <a href='{confirmationLink}'>Confirmation Email</a>";

                                //mail.Body = "<p>Veuillez cliquer sur le bouton suivant pour confirmer votre inscription.</p>" +
                                //            "<a class=\"btn btn-primary\"  asp-action=\"Confirm\"" + etudiant.Id + "' > Confirmation d'adresse! </a>";
                                mail.BodyEncoding = Encoding.UTF8;

                                mail.IsBodyHtml = true;

                                // Configurer le client SMTP
                                smtpClient.EnableSsl = true;
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                                // Envoyer l'e-mail
                                smtpClient.Send(mail);

                              
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {

                                ViewBag.Alert = "Une erreur est survenue, veuillez réessayer ultérieurement!";
                                return View();


                            }

                           
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
                ViewBag.Confirmation = "Veuillez verifier votre compte gmail pour la confirmation ";
                //return RedirectToAction("Login");
                return View(user);

                
            }
            return View(user);
        }

        [Authentification]
        public IActionResult Parametres()
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
                
                    string login = HttpContext.Session.GetString("Login");
                    Utilisateur utilisateur = db.Utilisateurs.Where(u => u.Login == login).Single();
                    if (utilisateur.Adresse != user.Adresse && db.Utilisateurs.Any(u => u.Email == user.Email))
                    {
                        ViewBag.msgValidation = "Email deja existant!!";
                    return View(user);
                }
                    utilisateur.Tel = user.Tel;
                    utilisateur.Email = user.Email;
                    
                    utilisateur.Adresse = user.Adresse;
                    utilisateur.Titre_Profil = user.Titre_Profil;

                    db.Utilisateurs.Update(utilisateur);
                    db.SaveChanges();
               
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
            TempData["login"] = mv.Login;
            TempData["password"] = mv.Password;
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == mv.Login && us.Password == mv.Password).FirstOrDefault();
                if (utilisateur != null)
                {
                    if(!utilisateur.IsComfirmed)
                    {
                        TempData["msgValidation"] = "Ce compte n'est pas encore Confirmé!";
                        return Json(new { username = mv.Login, password = mv.Password, redirectToUrl = Url.Action("Login", "User") });
                    }
                    else if (utilisateur.Isvalide == false)
                    {
                        TempData["msgValidation"] = "Ce compte n'est pas encore validé!";
                        return Json(new { username = mv.Login, password = mv.Password, redirectToUrl = Url.Action("Login", "User") });
                    }
                    else
                    {
                        HttpContext.Session.SetString("Login", utilisateur.Login);
                        HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                        HttpContext.Session.SetString("photo", utilisateur.Photo_Profil);
                      
                        utilisateur.date_Login= DateTime.Now;
                        db.Utilisateurs.Update(utilisateur);
                        db.SaveChanges();
                        if (HttpContext.Session.GetString("Role") == "AgentDirection")
                        {
                            return Json(new { redirectToUrl = Url.Action("Index", "Dashboard") });  
                        }
                        else
                        {
                            return Json(new { redirectToUrl = Url.Action("Annonces", "Annonce") }); 
                        }
                       

                    }


                }
                else
                {
                    TempData["msgErreur"] = "Login ou Mot de passe incorrect";
                }
            }

            return Json(new { username = mv.Login, password = mv.Password, redirectToUrl = Url.Action("Login", "User") });
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
        [HttpPost]
        public IActionResult SearchUsers(string term)
        {
            var res = db.Utilisateurs.Where(u => u.Nom.ToUpper().Contains(term) || u.Prenom.ToUpper().Contains(term)).ToList();
            return View(res);
        }


    }
}
