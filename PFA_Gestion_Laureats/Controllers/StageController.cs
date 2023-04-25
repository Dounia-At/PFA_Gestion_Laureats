using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;

using PFA_Gestion_Laureats.ViewModels.Stages;

namespace PFA_Gestion_Laureats.Controllers
{
    public class StageController : Controller
    {
        MyContext db;

        public StageController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Add()
        {

            List<Entreprise> entreprises = db.Entreprises.ToList();
            ViewBag.entreprises = new SelectList(entreprises, "Id", "Nom");


            return View();
        }
        [HttpPost]
        public IActionResult Add(AddStageViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            if (ModelState.IsValid)
            {
                

                Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();
                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise).FirstOrDefault();
               
                if (entreprise == null)
                {
                    Entreprise en = new Entreprise(amv.Entreprise);
                    db.Entreprises.Add(en);
                    db.SaveChanges();
                    entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
                }
                amv.EtudiantId = etudiant.Id;
                amv.EntrepriseId = entreprise.Id;

              

                Stage stage = new Stage(amv);
                stage.Etudiant = etudiant;
                stage.entreprise = entreprise;


                db.Stages.Add(stage);
                db.SaveChanges();

                return RedirectToAction("Details", "User", new { id = login });

            }
            return View();
           
        }
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();

            Stage stage = db.Stages.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (stage != null)
            {
                db.Stages.Remove(stage);
                db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = login });
            }


            return RedirectToAction("Details", "User", new { id = login });

        }
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            Stage stage = db.Stages.Include(af => af.Etudiant).Include(af=>af.entreprise).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (stage != null)
            {
                UpdateStageViewModel StageViewModel = new UpdateStageViewModel(stage);
                return View(StageViewModel);
            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]
        public IActionResult Update(UpdateStageViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();

            if (ModelState.IsValid)
            {
               
                Stage stage = db.Stages.Include(af => af.Etudiant).Where(af => af.Id == amv.Id && af.Etudiant.Id == etudiant.Id).FirstOrDefault();
                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == amv.Entreprise).FirstOrDefault();
                if (entreprise == null)
                {
                    Entreprise en = new Entreprise(amv.Entreprise);
                    db.Entreprises.Add(en);
                    db.SaveChanges();
                    entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
                }
                amv.EtudiantId = etudiant.Id;
                amv.EntrepriseId = entreprise.Id;

                if (stage != null)
                {
                    stage.Intitulé_poste = amv.Intitulé_poste;
                    stage.Description = amv.Description;
                    stage.Date_Debut = amv.Date_Debut;
                    stage.Date_Fin = amv.Date_Fin;
                    stage.entreprise = entreprise;
                    db.Stages.Update(stage);
                    db.SaveChanges();
                    return RedirectToAction("Details", "User", new { id = login });

                }



            }

            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]

        public JsonResult GetSearchResults(string Prefix)
        {
            var res = db.Entreprises.Where(en => en.Nom.ToUpper().Contains(Prefix)).Select(en => en.Nom).ToList();
            return Json(res);
        }
        public JsonResult ContenuSupplementaire()
        {
           
            List<Stage> contenuSupplementaire = db.Stages.ToList();
            return Json(contenuSupplementaire);
        }
    }
}
