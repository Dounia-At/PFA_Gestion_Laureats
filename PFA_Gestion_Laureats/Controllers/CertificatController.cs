using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.ViewModels.Certificats;

namespace PFA_Gestion_Laureats.Controllers
{
    public class CertificatController : Controller
    {
        MyContext db;

        public CertificatController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddCertificatViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            if (ModelState.IsValid)
            {

                Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();



                Certificat  certificat= new Certificat(amv);
                certificat.Etudiant = etudiant;



                db.Certificat.Add(certificat);
                db.SaveChanges();



            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();

            Certificat certificat = db.Certificat.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (certificat != null)
            {
                db.Certificat.Remove(certificat);
                db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = login });
            }


            return RedirectToAction("Details", "User", new { id = login });

        }
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Certificat certificat = db.Certificat.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (certificat != null)
            {
                UpdateCertificatViewModel certificatViewModel = new UpdateCertificatViewModel(certificat);
                return View(certificatViewModel);
            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]
        public IActionResult Update(UpdateCertificatViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Certificat certificat = db.Certificat.Include(af => af.Etudiant).Where(af => af.Id == amv.Id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();


            if (ModelState.IsValid)
            {

                if (certificat != null)
                {
                    certificat.Nom = amv.Nom;
                    certificat.Organisation = amv.Organisation;
                    certificat.Date_Emission = amv.Date_Emission;
                    certificat.Date_Expiration = amv.Date_Expiration;
                    certificat.Url = amv.Url;
                    db.Certificat.Update(certificat);
                    db.SaveChanges();
                    return RedirectToAction("Details", "User", new { id = login });

                }



            }

            return RedirectToAction("Details", "User", new { id = login });
        }
    }

}
