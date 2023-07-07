using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Services;
using PFA_Gestion_Laureats.ViewModels.Projets;

namespace PFA_Gestion_Laureats.Controllers
{
    public class ProjetController : Controller
    {
        MyContext db;
        
        public ProjetController(MyContext db)
        {
            this.db = db;
        }

        [Route("/Projet/Index/{idEtudiant}")]
        public IActionResult Index(string idEtudiant)
        {

            IList<Projet> projets = db.Projets.Include(s => s.Etudiant).Where(s => s.Etudiant.Login == idEtudiant).ToList();

            ViewBag.login = idEtudiant;
            return View(projets);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddProjetViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            if (ModelState.IsValid)
            {
               
                Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();
               
               

                    Projet p = new Projet(amv);
                    p.Etudiant = etudiant;
                  
                   

                    db.Projets.Add(p);
                    db.SaveChanges();

               

            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
           
                Projet projet = db.Projets.Include(ap => ap.Etudiant).Where(ap => ap.Id == id && ap.Etudiant.Id == utilisateur.Id).FirstOrDefault();
                if (projet != null)
                {
                    db.Projets.Remove(projet);
                    db.SaveChanges();
                  return RedirectToAction("Details", "User", new { id = login });
                 }


            return RedirectToAction("Details", "User", new { id = login });

        }
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Projet p = db.Projets.Include(an => an.Etudiant).Where(ap => ap.Id == id && ap.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (p != null)
            {
                UpdateProjetViewModel ProjetViewModel = new UpdateProjetViewModel(p);
                return View(ProjetViewModel);
            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]
        public IActionResult Update(UpdateProjetViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Projet projet = db.Projets.Include(an => an.Etudiant).Where(ap => ap.Id == amv.Id && ap.Etudiant.Id == utilisateur.Id).FirstOrDefault();


            if (ModelState.IsValid)
            {

                if (projet != null)
                {
                    projet.Nom = amv.Nom;
                    projet.Description = amv.Description;
                    projet.Date_Debut = amv.Date_Debut;
                    projet.Date_Fin = amv.Date_Fin;
                    db.Projets.Update(projet);
                    db.SaveChanges();
                    return RedirectToAction("Details", "User", new { id = login });

                }



            }

            return RedirectToAction("Details", "User", new { id = login });
        }
    }
}
