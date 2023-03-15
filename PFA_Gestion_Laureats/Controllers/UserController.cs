using Microsoft.AspNetCore.Mvc;

namespace PFA_Gestion_Laureats.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
