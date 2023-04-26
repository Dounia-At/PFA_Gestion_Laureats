using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using PFA_Gestion_Laureats.ViewModels.Entreprises;

namespace PFA_Gestion_Laureats.Controllers
{
    [Authentification]
    public class EntrepriseController : Controller
    {
        MyContext db;
        public EntrepriseController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Role") == "Agent")
            {
                ViewBag.role = "Agent";
            }
            List<Entreprise> Entreprises = db.Entreprises.AsNoTracking().ToList();
            return View(Entreprises);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(EntrepriseViewModel entrepriseView)
        {
            if(entrepriseView.Logo == null)
            {
                entrepriseView.Logo = "entrepriseLogo.jpg";
            }
            if (ModelState.IsValid)
            {
                Entreprise entreprise = new Entreprise();
                entreprise.Nom = entrepriseView.Nom;
                entreprise.Pays = entrepriseView.Pays;
                entreprise.Ville = entrepriseView.Ville;
                entreprise.Adresse = entrepriseView.Adresse;
                entreprise.Description = entrepriseView.Description;
                entreprise.Convention = (bool)entrepriseView.Convention;

                string NewName = Guid.NewGuid() + entrepriseView.Photo.FileName;
                string PathFile = Path.Combine("wwwroot/assets/img/logo", NewName);

                using (FileStream stream = System.IO.File.Create(PathFile))
                {
                    entrepriseView.Photo.CopyTo(stream);
                    entreprise.Logo = NewName;
                }

                db.Entreprises.Add(entreprise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entrepriseView);
        }
        public IActionResult Update(int id)
        {
            Entreprise entreprise = db.Entreprises.Find(id);
            EntrepriseViewModel entrepriseView = new EntrepriseViewModel(entreprise.Id, entreprise.Nom,
                                                                            entreprise.Pays, entreprise.Ville,
                                                                            entreprise.Adresse, entreprise.Logo,
                                                                            entreprise.Description, entreprise.Convention);
            return View(entrepriseView);
        }
        [HttpPost]
        public IActionResult Update(EntrepriseViewModel entrepriseView)
        {
            if (ModelState.IsValid)
            {
                Entreprise entreprise = db.Entreprises.Find(entrepriseView.Id);
                entreprise.Nom = entrepriseView.Nom;
                entreprise.Pays = entrepriseView.Pays;
                entreprise.Ville = entrepriseView.Ville;
                entreprise.Adresse = entrepriseView.Adresse;
                entreprise.Convention = (bool)entrepriseView.Convention;
                if(entrepriseView.Logo != null)
                {
                    System.IO.File.Delete(Path.Combine("wwwroot/assets/img/logo", entreprise.Logo));
                    string NewName = Guid.NewGuid() + entrepriseView.Photo.FileName;
                    string PathFile = Path.Combine("wwwroot/assets/img/logo", NewName);

                    using (FileStream stream = System.IO.File.Create(PathFile))
                    {
                        entrepriseView.Photo.CopyTo(stream);
                        entreprise.Logo = NewName;
                    }
                }
               

                db.Entreprises.Update(entreprise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entrepriseView);
        }
        public IActionResult Delete(int id)
        {
            Entreprise Entreprise = db.Entreprises.Find(id);

            db.Entreprises.Remove(Entreprise);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}
