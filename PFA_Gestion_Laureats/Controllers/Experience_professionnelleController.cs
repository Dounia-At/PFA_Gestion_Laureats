using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.ViewModels;
using PFA_Gestion_Laureats.ViewModels.ExperiencePros;

namespace PFA_Gestion_Laureats.Controllers
{
    public class Experience_professionnelleController : Controller
    {
        MyContext db;
        public Experience_professionnelleController(MyContext db)
        {
            this.db = db;
        }

        [Route("/Experience_professionnelle/Index/{idEtudiant}")]
        public IActionResult Index(string idEtudiant)
        {

            IList<ExperiencePro> experiences = db.ExperiencePro.Include(s => s.entreprise).Include(s => s.Etudiant).Where(s => s.Etudiant.Login == idEtudiant).ToList();

            ViewBag.login = idEtudiant;
            return View(experiences);
        }

        public IActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Add(AddExperienceProViewModel amv)
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
                    entreprise= db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
                }
                amv.EtudiantId = etudiant.Id;
                amv.EntrepriseId = entreprise.Id;



                ExperiencePro experiencePro = new ExperiencePro(amv);

                experiencePro.Etudiant = etudiant;
                experiencePro.entreprise = entreprise;


                db.ExperiencePro.Add(experiencePro);
                db.SaveChanges();

                return RedirectToAction("Details", "User", new { id = login });

            }
            return View();

        }
        public IActionResult Delete(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();

            ExperiencePro experiencePro = db.ExperiencePro.Include(af => af.Etudiant).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (experiencePro != null)
            {
                db.ExperiencePro.Remove(experiencePro);
                db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = login });
            }


            return RedirectToAction("Details", "User", new { id = login });

        }
        public IActionResult Update(int id)
        {
            String login = HttpContext.Session.GetString("Login");
            Utilisateur utilisateur = db.Utilisateurs.Where(us => us.Login == login).FirstOrDefault();
            ExperiencePro experiencePro = db.ExperiencePro.Include(af => af.Etudiant).Include(af=>af.entreprise).Where(af => af.Id == id && af.Etudiant.Id == utilisateur.Id).FirstOrDefault();
            if (experiencePro != null)
            {
                UpdateExperienceProViewModel experienceProViewModel = new UpdateExperienceProViewModel(experiencePro);
                return View(experienceProViewModel);
            }
            return RedirectToAction("Details", "User", new { id = login });
        }
        [HttpPost]
        public IActionResult Update(UpdateExperienceProViewModel amv)
        {
            String login = HttpContext.Session.GetString("Login");
            Etudiant etudiant = db.Etudiants.Where(us => us.Login == login).FirstOrDefault();
           

            if (ModelState.IsValid)
            {
                ExperiencePro experiencePro = db.ExperiencePro.Include(af => af.Etudiant).Where(af => af.Id == amv.Id && af.Etudiant.Id == etudiant.Id).FirstOrDefault();
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

                if (experiencePro != null)
                {
                    experiencePro.Post = amv.Post;
                    experiencePro.Type_Emploi = amv.Type_Emploi;
                    experiencePro.Etat = experiencePro.Etat;
                  
                    experiencePro.entreprise = entreprise;
                    db.ExperiencePro.Update(experiencePro);
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
    }
}
