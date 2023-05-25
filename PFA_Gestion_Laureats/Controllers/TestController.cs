using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Validation;
using PFA_Gestion_Laureats.ViewModels.Tests;

namespace PFA_Gestion_Laureats.Controllers
{
    [Authentification]
    public class TestController : Controller
    {
        MyContext db;
        public TestController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "AgentDirection")
            {
                ViewBag.role = "AgentDirection";
            }
            if (db.Entreprises.Count() == 0) return RedirectToAction("Index", "Entreprise");
            if (db.Agents.Count() == 0) return RedirectToAction("Add_Agent", "User");
            List<Test> tests = db.Tests.OrderByDescending(t => t.Id).Include(t => t.entreprise).Include(t => t.agentDirection).AsNoTracking().ToList();
            

            return View(tests);
        }
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("Role") == "AgentDirection")
            {
                ViewBag.role = "AgentDirection";
            }
            Test test = db.Tests.Find(id);
            test.entreprise = db.Entreprises.Find(test.EntrepriseId);
            test.agentDirection = db.Agents.Find(test.AgentDirectionId);

            return View(test);
        }
        public IActionResult Add()
        {
            ViewBag.entreprises = new SelectList(db.Entreprises.ToList(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        public IActionResult Add(TestViewModel testView)
        {
            Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == testView.Entreprise).FirstOrDefault();

            if (entreprise == null)
            {
                Entreprise en = new Entreprise(testView.Entreprise);
                db.Entreprises.Add(en);
                db.SaveChanges();
                entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
            }

            testView.EntrepriseId = entreprise.Id;
            if (ModelState.IsValid)
            {
                Test test = new Test();
                test.Date_Test = testView.Date_Test;
                test.Description = testView.Description;
                test.Heure_Test = testView.Heure_Test;
                test.EntrepriseId = testView.EntrepriseId;
                test.agentDirection = db.Agents.Where(a => a.Login == HttpContext.Session.GetString("Login")).SingleOrDefault();
                test.AgentDirectionId = test.agentDirection.Id;

                db.Tests.Add(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.entreprises = new SelectList(db.Entreprises.ToList(), "Id", "Nom", testView.EntrepriseId);

            return View(testView);
        }
        public IActionResult Update(int id)
        {
            
            Test test = db.Tests.Find(id);
            TestViewModel testView = new TestViewModel(test.Id, test.Date_Test, test.Heure_Test, 
                                                        test.Description, test.EntrepriseId );
            ViewBag.entreprises = new SelectList(db.Entreprises.ToList(), "Id", "Nom", testView.EntrepriseId);

            return View(testView);
        }
        [HttpPost]
        public IActionResult Update(TestViewModel testView)
        {
            if (ModelState.IsValid)
            {
                Entreprise entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == testView.Entreprise).FirstOrDefault();

                if (entreprise == null)
                {
                    Entreprise en = new Entreprise(testView.Entreprise);
                    db.Entreprises.Add(en);
                    db.SaveChanges();
                    entreprise = db.Entreprises.Where(ae => ae.Nom.ToUpper() == en.Nom).FirstOrDefault();
                }
                testView.EntrepriseId = entreprise.Id;
                Test test = db.Tests.Find(testView.Id);
                test.Date_Test = testView.Date_Test;
                test.Description = testView.Description;
                test.Heure_Test = testView.Heure_Test;
                test.EntrepriseId = testView.EntrepriseId;

                test.agentDirection = db.Agents.Where(a => a.Login == HttpContext.Session.GetString("Login")).SingleOrDefault();
                test.AgentDirectionId = test.agentDirection.Id;
                               

                db.Tests.Update(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.entreprises = new SelectList(db.Entreprises.ToList(), "Id", "Nom", testView.EntrepriseId);

            return View(testView);
        }
        public IActionResult Delete(int id)
        {
            Test test = db.Tests.Find(id);

            db.Tests.Remove(test);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult GetSearchResults(string Prefix)
        {
            var res = db.Entreprises.Where(en => en.Nom.ToUpper().Contains(Prefix)).Select(en => en.Nom).ToList();
            return Json(res);
        }
    }
}
