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
using System.Text.RegularExpressions;

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

        public IActionResult NoFiltre()
        {
            List<Annonce> annonces = db.Annonces.Include(annonce => annonce.utilisateur).Include(annonce => annonce.entreprise).Include(annonce => annonce.AnnonceTechnologies).Include(annonce => annonce.postulations).OrderByDescending(an => an.Date_Creation).AsNoTracking().ToList();
            return RedirectToAction("Annonces", annonces);
        }
        public IActionResult Add()
        {
            
                List<Entreprise>entreprises=db.Entreprises.ToList();
               ViewBag.entreprises = new SelectList(entreprises, "Id", "Nom");
            List<Technologie> technologies = db.Technologie.ToList();
            ViewBag.technologies = new SelectList(technologies, "Id", "Libelle");


            return View();
        }
        [HttpPost] 
        public IActionResult Add(AddAnnonceViewModel amv,string[] technologies) {
            if(ModelState.IsValid)
            {
                String login = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
                //Entreprise entreprise = db.Entreprises.Where(ae=>ae.Id==amv.EntrepriseId).FirstOrDefault();
                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise.ToUpper()).FirstOrDefault();
                
                if (utilisateur is IModirateur)
                {
                    if (entreprise == null)
                    {
                        Entreprise en = new Entreprise(amv.Entreprise);
                        db.Entreprises.Add(en);
                        db.SaveChanges();
                        entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
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
                    int annonceId = db.Annonces.Where(a => a.Date_Creation == annonce1.Date_Creation).Select(a => a.Id).FirstOrDefault();
                    foreach (string item in technologies)
                    {                                                                         
                            AnnonceTechnologie at = new AnnonceTechnologie();
                            at.AnnonceId = annonceId;
                            at.TechnologieId = int.Parse(item);
                            db.AnnonceTechnologies.Add(at);
                                                 
                    }
                    db.SaveChanges();

                }
               

            }
            return RedirectToAction("MesAnnonces");
        }
        public IActionResult Annonces(string SearchString, string Nature,string remunerer,string Technologie, string orderBy)
        {
            ViewBag.SearchString = SearchString;
            ViewBag.Nature = Nature;
            ViewBag.Remunerer = remunerer;
            ViewBag.Technologie = Technologie;


            //if (db.Entreprises.Count() == 0) return RedirectToAction("Index", "Entreprise");
            List<Annonce> annonces = db.Annonces.Include(annonce => annonce.utilisateur).Include(annonce => annonce.entreprise).Include(annonce => annonce.AnnonceTechnologies).ThenInclude(an=> an.Technologie).Include(annonce => annonce.postulations).OrderByDescending(an=>an.Date_Creation).AsNoTracking().ToList();
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                annonces = annonces.Where(a => a.entreprise.Nom.ToLower().Contains(SearchString.ToLower()) || a.entreprise.Ville.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Nature))
            {
                annonces = annonces.Where(a => a.Nature==Nature).ToList();
            }
            if (!string.IsNullOrEmpty(remunerer))
            {
                if(remunerer== "Remunerer")
                {
                    annonces = annonces.Where(a => a.Remuniration == true).ToList();
                }
                else if (remunerer == "NonRemunerer")
                {
                    annonces = annonces.Where(a => a.Remuniration == false).ToList();
                }
               
            }
            
            if (!string.IsNullOrEmpty(Technologie))
            {
                List<Annonce> annonces1 = new List<Annonce>();
                foreach (Annonce annonce in annonces.ToList())
                {
                    foreach (AnnonceTechnologie technologie in annonce.AnnonceTechnologies)
                    {
                        if (technologie.Technologie.Libelle.Equals(Technologie))
                        {
                            annonces1.Add(annonce);
                        }
                    }
                }
                annonces = annonces1;
            }

            if (!string.IsNullOrEmpty(orderBy))
            {                                           
                switch (orderBy)
                {                   
                    case "plusConsultes":
                       
                        annonces = annonces.OrderByDescending(a => a.postulations.Count).ToList();
                        break;
                  
                    case "plusPostules":
                       
                        annonces = annonces.OrderByDescending(a => a.postulations.Count(p=> p.Date_Postulation != null)).ToList();
                        break;
                    case "moinsConsultes":
                       
                        annonces = annonces.OrderBy(a => a.postulations.Count).ToList();
                        break;
                    case "moinsPostules":

                        annonces = annonces.OrderBy(a => a.postulations.Count(p => p.Date_Postulation != null)).ToList();
                        break;

                   
                }
            }

            return View(annonces);
        }
        [Authentification]
        public IActionResult Details(int id)
        { 
            Annonce annonce=db.Annonces.Include(annonce => annonce.utilisateur).Where(an=>an.Id==id).FirstOrDefault();
            if (annonce != null)
            {
                string login = HttpContext.Session.GetString("Login");
                int utilisateurId = db.Utilisateurs.Where(us => us.Login == login).Select(u => u.Id).FirstOrDefault();
                Postulation p = db.Postulations.Where(p => p.AnnonceId == annonce.Id && p.EtudiantId == utilisateurId).FirstOrDefault();
                if (p == null)
                {
                    p = new Postulation();
                    p.EtudiantId = utilisateurId;
                    p.AnnonceId = annonce.Id;
                    p.Date_Consultation = DateTime.Now;
                    db.Postulations.Add(p);
                    db.SaveChanges();
                }
                an = annonce;

                return View(annonce);
            }

            return RedirectToAction("Annonces");
        }
        [Authentification]
        public IActionResult MesAnnonces()
        {
            string login = HttpContext.Session.GetString("Login");
            List<Annonce> annonces = db.Annonces.Include(annonce => annonce.utilisateur).Include(annonce => annonce.postulations).Include(annonce=>annonce.entreprise).Where(an => an.utilisateur.Login == login).OrderByDescending(an => an.Date_Creation).AsNoTracking().ToList();
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
            Annonce annonce = db.Annonces.Include(an => an.utilisateur).Include(an=> an.AnnonceTechnologies).Where(an => an.Id == id && an.utilisateur.Id == utilisateur.Id).FirstOrDefault();
            if (annonce != null)
            {
                UpdateAnnonceViewModel annonceViewModel = new UpdateAnnonceViewModel(annonce);
                ViewBag.entreprise = db.Entreprises.Where(e=> e.Id == annonce.EntrepriseId).Select(e=> e.Nom).FirstOrDefault();
                ViewBag.technologies = new SelectList(db.Technologie.ToList(), "Id", "Libelle");
                annonceViewModel.Technologies = db.Technologie.ToList();
                annonceViewModel.SelectedTechnologies = annonce.AnnonceTechnologies.Select(t => t.TechnologieId).ToList();

                return View(annonceViewModel);
            }
            return RedirectToAction("Annonces");
        }
        [HttpPost]
        public IActionResult Update(UpdateAnnonceViewModel amv, string[] technologies)
        {

            if (ModelState.IsValid)
            {
                String login = HttpContext.Session.GetString("Login");
                Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
                Annonce annonce = db.Annonces.Include(an => an.utilisateur).Include(a => a.AnnonceTechnologies).Where(an => an.Id == amv.Id && an.utilisateur.Id == utilisateur.Id).FirstOrDefault();

                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise.ToUpper()).FirstOrDefault();

                if (entreprise == null)
                {
                    Entreprise en = new Entreprise(amv.Entreprise);
                    db.Entreprises.Add(en);
                    db.SaveChanges();
                    entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
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
                    foreach (AnnonceTechnologie techno in annonce.AnnonceTechnologies)
                    {
                        db.AnnonceTechnologies.Remove(techno);
                    }
                    
                    foreach (string item in technologies)
                    {
                        AnnonceTechnologie at = new AnnonceTechnologie();
                        at.AnnonceId = annonce.Id;
                        at.TechnologieId = int.Parse(item);
                        db.AnnonceTechnologies.Add(at);
                    }
                    db.SaveChanges();                                       
                }                
            }
            return RedirectToAction("MesAnnonces");
        }
        public ActionResult Postuler()
        {
            string login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            ViewBag.email = utilisateur.Email;
            return View(); 
        }
            [HttpPost]
        public ActionResult Postuler(AddPostulerViewModel pmv)
        {
            string login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
         
            if (ModelState.IsValid)
            {
                Postulation p = db.Postulations.Where(p => p.AnnonceId == an.Id && p.EtudiantId == utilisateur.Id).FirstOrDefault();
                
                    p.Date_Postulation = DateTime.Now;
                    db.Postulations.Add(p);
                    db.SaveChanges();
                
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
        public JsonResult GetSearchResults(string Prefix)
        {
            var res = db.Entreprises.Where(en => en.Nom.ToUpper().Contains(Prefix)).Select(en => en.Nom).ToList();
            return Json(res);
        }
        public JsonResult GetSearchTechnologies(string Prefix)
        {
            var res = db.Technologie.Where(en => en.Libelle.ToUpper().Contains(Prefix)).Select(en => en.Libelle).ToList();
            return Json(res);
        }
    }
}
