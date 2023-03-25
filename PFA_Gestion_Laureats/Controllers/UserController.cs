using Microsoft.AspNetCore.Mvc;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.ViewModels;

namespace PFA_Gestion_Laureats.Controllers
{
    public class UserController : Controller
    {
        private MyContext db;
        public UserController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel mv)
        {
            if(ModelState.IsValid)
            {
                Utilisateur utilisateur=db.Utilisateurs.Where(us=>us.Login==mv.Login &&us.Password==mv.Password ).FirstOrDefault();
                if (utilisateur != null)
                {
                    if (utilisateur.Isvalide == false)
                    {
                        ViewBag.msgValidation = "Ce compte n'est pas encore validé";
                    }
                    else
                    {
                        HttpContext.Session.SetString("login", utilisateur.Login);
                        HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                       
                        
                        if (utilisateur.GetType().Name == "AgentDirection")
                        {
                            HttpContext.Session.SetString("Role", "Agent");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Role", utilisateur.GetType().Name);
                        }
                       
                        return RedirectToAction("Index", "Home");

                    }
                   
                    

                }
                else
                {
                    ViewBag.msgErreur = "Login ou Mot de passe incorrect";
                }
            }
           
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("login");
            return RedirectToAction("Login", "User");
        }
    }
}
