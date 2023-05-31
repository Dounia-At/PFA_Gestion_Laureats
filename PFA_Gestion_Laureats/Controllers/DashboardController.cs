using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.Controllers
{
    public class DashboardController : Controller
    {
        MyContext db;
        public DashboardController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var NbrEtudiant = db.Etudiants.Count();
            ViewBag.NbrEtudiant = NbrEtudiant;

            var NbrEntreprise = db.Entreprises.Count();
            ViewBag.NbrEntreprise = NbrEntreprise;

            var NbrAnnonces = db.Annonces.Count();
            ViewBag.NbrAnnonces = NbrAnnonces;

            var NbrAnnoncesStages = db.Annonces.Count(a=> a.Nature.Equals("PFA") || a.Nature.Equals("PFE"));
            ViewBag.NbrAnnoncesStages = NbrAnnoncesStages;

            var NbrAnnoncesEmploie = db.Annonces.Count(a=> !a.Nature.Equals("PFA") && !a.Nature.Equals("PFE"));
            ViewBag.NbrAnnoncesEmploie = NbrAnnoncesEmploie;
            
            var NbrTest = db.Tests.Count();
            ViewBag.NbrTest = NbrTest;
            
           
            
            var NbrPostulation= db.Postulations.Count();
            ViewBag.NbrPostulation = NbrPostulation;
            
            var NbrConsultation = db.Postulations.Count(p=> p.Date_Consultation!=null);
            ViewBag.NbrConsultation = NbrConsultation;




            var workingCount = db.ExperiencePro.Where(s => s.Etat == true).GroupBy(p => p.EtudiantId).Count();//NbrEtudiantEmployé
            var notWorkingCount = db.ExperiencePro.Where(s => s.Etat == false).GroupBy(p => p.EtudiantId).Count();
            ViewBag.WorkingCount = workingCount;
            ViewBag.NotWorkingCount = notWorkingCount;



            var SansStage=db.Etudiants.Include(s=>s.stages).Where(s=>s.stages.Count()== 0).Count();
            var UnStage = db.Etudiants.Include(s => s.stages).Where(s => s.stages.Count() == 1).Count();
            var PlusStage = db.Etudiants.Include(s => s.stages).Where(s => s.stages.Count() > 1).Count();
            ViewBag.SansStage = SansStage;
            ViewBag.UnStage = UnStage;
            ViewBag.PlusStage = PlusStage;
            
            var SansCertificat=db.Etudiants.Include(s=>s.certificats).Where(s=>s.certificats.Count()== 0).Count();
            var UneCertificat = db.Etudiants.Include(s => s.certificats).Where(s => s.certificats.Count() == 1).Count();
            var PlusCertificat = db.Etudiants.Include(s => s.certificats).Where(s => s.certificats.Count() > 1).Count();
            ViewBag.SansCertificat = SansCertificat;
            ViewBag.UneCertificat = UneCertificat;
            ViewBag.PlusCertificat = PlusCertificat;


            var SansProjet = db.Etudiants.Include(s => s.projets).Where(s => s.projets.Count() == 0).Count();
            var UnProjet = db.Etudiants.Include(s => s.projets).Where(s => s.projets.Count() == 1).Count();
            var PlusProjet = db.Etudiants.Include(s => s.projets).Where(s => s.projets.Count() > 1).Count();
            ViewBag.SansProjet = SansProjet;
            ViewBag.UnProjet = UnProjet;
            ViewBag.PlusProjet = PlusProjet;

            var SansFormation = db.Etudiants.Include(s => s.formations).Where(s => s.formations.Count() == 0).Count();
            var UneFormation = db.Etudiants.Include(s => s.formations).Where(s => s.formations.Count() == 1).Count();
            var PlusFormation = db.Etudiants.Include(s => s.formations).Where(s => s.formations.Count() > 1).Count();
            ViewBag.SansFormation = SansFormation;
            ViewBag.UneFormation = UneFormation;
            ViewBag.PlusFormation = PlusFormation;

            var annoncesByMonth = db.Annonces
            .GroupBy(s => new { s.Date_Creation.Year, s.Date_Creation.Month })
            .Select(g => new
            {
                year = g.Key.Year,
                month = g.Key.Month,
                annonceCount = g.Count()
            })
            .OrderBy(g => g.year)
            .ThenBy(g => g.month)
            .ToList();

            ViewBag.annoncesByMonth = annoncesByMonth;


            var top10Entreprises = db.Entreprises
           .OrderByDescending(s => s.annonces.Count()) 
           .Take(10)
           .ToList();
            ViewBag.top10Entreprises= top10Entreprises;

            var top10Technologies = db.Technologie
          .OrderByDescending(s => s.AnnonceTechnologies.Count())
          .Take(10)
          .ToList();
            ViewBag.top10Technologies = top10Technologies;

            var top10Engagement = db.Entreprises
          .OrderByDescending(s => (s.stages.Count() + s.experiences.Count()))
          .Take(10)
          .ToList();
            ViewBag.top10Engagement = top10Engagement;
            return View();
        }
    }
}
