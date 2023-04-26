using MailKit.Security;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Services;
using PFA_Gestion_Laureats.Validation;
using System.Linq;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PFA_Gestion_Laureats.ViewModels.Annonces;

namespace PFA_Gestion_Laureats.Controllers
{

    public class AnnonceController : Controller
    {
        MyContext db;
        static  Annonce an;
        public AnnonceController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Add()
        {
            
                List<Entreprise>entreprises=db.Entreprises.ToList();
               ViewBag.entreprises = new SelectList(entreprises, "Id", "Nom");


            return View();
        }
        [HttpPost] 
        public IActionResult Add(AddAnnonceViewModel amv) {
            if(ModelState.IsValid)
            {
                String login = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
                //Entreprise entreprise = db.Entreprises.Where(ae=>ae.Id==amv.EntrepriseId).FirstOrDefault();
                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise).FirstOrDefault();
               
                if (utilisateur is IModirateur)
                {
                    if (entreprise == null)
                    {
                        Entreprise en = new Entreprise(amv.Entreprise);
                        db.Entreprises.Add(en);
                        db.SaveChanges();
                    }
                   
                    amv.EntrepriseId = entreprise.Id;
                    Annonce annonce1 = new Annonce(amv);
                    annonce1.utilisateur = utilisateur;
                    annonce1.entreprise = entreprise;
                    if (amv.Photo != null)
                    {
                        string[] AllowedExt = { ".png", ".jpg", ".jpeg" };

                        string FileExt = Path.GetExtension(amv.Photo.FileName);
                        if (AllowedExt.Contains(FileExt.ToLower()))
                        {
                            string NewName = Guid.NewGuid() + amv.Photo.FileName;
                            string PathFile = Path.Combine("wwwroot/assets/img/Annones", NewName);

                            using (FileStream stream = System.IO.File.Create(PathFile))
                            {
                                amv.Photo.CopyTo(stream);
                                annonce1.Photo = NewName;
                            }

                        }
                        
                    }
                   
                    db.Annonces.Add(annonce1);
                    db.SaveChanges();
                }
               

            }
            return RedirectToAction("Annonces");
        }
        public IActionResult Annonces()
        {
          List<Annonce>annonces=db.Annonces.Include(annonce=>annonce.utilisateur).Include(annonce => annonce.entreprise).OrderByDescending(an=>an.Date_Creation).AsNoTracking().ToList();
            return View(annonces);
        }
        public IActionResult Details(int id)
        { 
            Annonce annonce=db.Annonces.Include(annonce => annonce.utilisateur).Where(an=>an.Id==id).FirstOrDefault();
            if (annonce != null)
            {
                an = annonce;
                return View(annonce);
            }

            return RedirectToAction("Annonces");
        }
            [Authentification]
        public IActionResult MesAnnonces()
        {
            String login = HttpContext.Session.GetString("Login");
            List<Annonce> annonces = db.Annonces.Include(annonce => annonce.utilisateur).Where(an => an.utilisateur.Login == login).OrderByDescending(an => an.Date_Creation).AsNoTracking().ToList();
            return View(annonces);
        }
        [Authentification]
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            if (utilisateur is IModirateur)
            {
                Annonce annonce = db.Annonces.Include(an=>an.utilisateur).Where(an => an.Id == id && an.utilisateur.Id==utilisateur.Id).FirstOrDefault();
                if (annonce != null)
                {
                    db.Annonces.Remove(annonce);
                    db.SaveChanges();
                    return RedirectToAction("Annonces");
                }
            }
               
            return RedirectToAction("Annonces");




        }
        [Authentification]
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Annonce annonce = db.Annonces.Include(an => an.utilisateur).Where(an => an.Id == id && an.utilisateur.Id == utilisateur.Id).FirstOrDefault();
            if (annonce != null)
            {
                UpdateAnnonceViewModel annonceViewModel = new UpdateAnnonceViewModel(annonce);
                return View(annonceViewModel);
            }
            return RedirectToAction("Annonces");
        }
        [HttpPost]
        public IActionResult Update(UpdateAnnonceViewModel amv)
        {
           
            if (ModelState.IsValid)
            {
                String login = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
                Annonce annonce = db.Annonces.Include(an => an.utilisateur).Where(an => an.Id == amv.Id && an.utilisateur.Id == utilisateur.Id).FirstOrDefault();

                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise).FirstOrDefault();

                if (entreprise == null)
                {
                    Entreprise en = new Entreprise(amv.Entreprise);
                    db.Entreprises.Add(en);
                    db.SaveChanges();
                }
              
                amv.EntrepriseId = entreprise.Id;

                if (annonce != null)
                {
                    annonce.Titre=amv.Titre;
                    annonce.Description=amv.Description;
                    annonce.Email_Reception=amv.Email_Reception;
                    //annonce.Photo=amv.Photo.FileName;
                    annonce.Date_limite_Deposer = amv.Date_limite_Deposer;
                    annonce.entreprise = entreprise;
                    if(amv.Photo!=null)
                    {
                        string[] AllowedExt = { ".png", ".jpg" , ".jpeg" };

                        string FileExt = Path.GetExtension(amv.Photo.FileName);
                        if (AllowedExt.Contains(FileExt.ToLower()))
                        {
                            string NewName = Guid.NewGuid() + amv.Photo.FileName;
                            string PathFile = Path.Combine("wwwroot/assets/img/Annones", NewName);

                            using (FileStream stream = System.IO.File.Create(PathFile))
                            {
                                amv.Photo.CopyTo(stream);
                                annonce.Photo = NewName;
                            }

                        }
                    }
                    db.Annonces.Update(annonce);
                    db.SaveChanges();

                }
                


            }
            return RedirectToAction("MesAnnonces");
        }
        public ActionResult Postuler()
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            ViewBag.email = utilisateur.Email;
            return View(); 
        }
            [HttpPost]
        public ActionResult Postuler(AddPostulerViewModel pmv)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
         
            if (ModelState.IsValid)
            {
                try
                {
                    using (MailMessage mail = new MailMessage(utilisateur.Email, an.Email_Reception))
                    {

                        mail.Subject = an.Titre;
                        mail.Body = "";

                        string fileName = Path.GetFileName(pmv.CV.FileName);
                        mail.Attachments.Add(new Attachment(pmv.CV.OpenReadStream(), fileName));

                        mail.IsBodyHtml = false;
                        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";

                            NetworkCredential NetworkCred = new NetworkCredential(utilisateur.Email, pmv.password);
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

                    ViewBag.Alert = "Impossible d'envoyer l'e-mail. Vérifiez votre Mot de Passe";
                    return View();
                     
                   
                }
              
            }


         

           

           
            return RedirectToAction("Annonces");

        }

    }
}
