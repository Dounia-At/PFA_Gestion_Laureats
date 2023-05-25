using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using System.Security.Claims;

namespace PFA_Gestion_Laureats.Controllers
{
    public class MessageController : Controller
    {
        MyContext db;
        public MessageController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            String login = HttpContext.Session.GetString("Login");
            List<Utilisateur> utilisateurs = db.Utilisateurs.Include(m => m.messagesEnvoyees).ToList();
            ViewBag.Id = db.Utilisateurs.Where(u => u.Login == login).Select(u => u.Id).FirstOrDefault();
            return View(utilisateurs);
        }
        
        public async Task<IActionResult> Chat(int receiverId)
        {
            string login = HttpContext.Session.GetString("Login");
            int senderId = db.Utilisateurs.Where(u => u.Login == login).Select(u => u.Id).FirstOrDefault();
            string receiverLogin = db.Utilisateurs.Where(u => u.Id == receiverId).Select(u => u.Login).FirstOrDefault();

            List<Message> messages = await db.Messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId)
                         || (m.SenderId == receiverId && m.ReceiverId == senderId)).Include(u => u.UtilisateurSender).Include(u => u.UtilisateurReceiver)
                .OrderBy(m => m.Date_Envoie)
                .ToListAsync();
           
            ViewData["ReceiverId"] = receiverLogin;
            ViewData["senderId"] = senderId;

            return View(messages);
        }

    }
}
