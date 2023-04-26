using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.ViewModels.Formations;

namespace PFA_Gestion_Laureats.Controllers
{
    public class FormationController : Controller
    {
        MyContext db;

        public FormationController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddFormationViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            if (ModelState.IsValid)
            {

                Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();



                Formation formation = new Formation(amv);
                formation.Etudiant = etudiant;



                db.Formations.Add(formation);
                db.SaveChanges();



            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();

            Formation formation = db.Formations.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (formation != null)
            {
                db.Formations.Remove(formation);
                db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = login });
            }


            return RedirectToAction("Details", "User", new { id = login });

        }
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Formation f = db.Formations.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (f != null)
            {
                UpdateFormationViewModel FormationViewModel = new UpdateFormationViewModel(f);
                return View(FormationViewModel);
            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]
        public IActionResult Update(UpdateFormationViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Formation formation = db.Formations.Include(af => af.Etudiant).Where(af => af.Id == amv.Id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();


            if (ModelState.IsValid)
            {

                if (formation != null)
                {
                    formation.Diplome = amv.Diplome;
                    formation.Description = amv.Description;
                    formation.Date_Debut = amv.Date_Debut;
                    formation.Date_Fin = amv.Date_Fin;
                    formation.Ecole = amv.Ecole;
                    db.Formations.Update(formation);
                    db.SaveChanges();
                    return RedirectToAction("Details", "User", new { id = login });

                }



            }

            return RedirectToAction("Details", "User", new { id = login });
        }
    }
}
