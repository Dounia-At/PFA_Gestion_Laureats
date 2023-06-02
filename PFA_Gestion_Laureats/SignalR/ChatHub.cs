using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using System.Security.Claims;

namespace PFA_Gestion_Laureats.SignalR
{
    public class ChatHub : Hub
    {
        private readonly MyContext _dbContext;
        

        public ChatHub(MyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitConnectionId()
        {
            var a = Context.User.Identity.Name;
            String Senderlogin = Context.GetHttpContext().Session.GetString("Login");
            Utilisateur sender = _dbContext.Utilisateurs.Where(u => u.Login == Senderlogin).FirstOrDefault();
            sender.ConnectionId = Context.ConnectionId;
            _dbContext.Utilisateurs.Update(sender);
            await _dbContext.SaveChangesAsync();

        }
        public override Task OnConnectedAsync()
        {
            string Senderlogin = Context.GetHttpContext().Session.GetString("Login");
            Utilisateur sender = _dbContext.Utilisateurs.Where(u => u.Login == Senderlogin).FirstOrDefault();
            sender.ConnectionId = Context.ConnectionId;
            _dbContext.Utilisateurs.Update(sender);
             _dbContext.SaveChangesAsync();
            return base.OnConnectedAsync();
        }
        
        public async Task SendMessage(string receiverLogin,string message)
        {
            string login = Context.GetHttpContext().Session.GetString("Login");
            int senderId = _dbContext.Utilisateurs.Where(u => u.Login == login).Select(u => u.Id).FirstOrDefault();
            Utilisateur receiver = _dbContext.Utilisateurs.Where(u => u.Login == receiverLogin).FirstOrDefault();

            Message newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                contenu = message,
                Date_Envoie = DateTime.UtcNow
            };

            _dbContext.Messages.Add(newMessage);


                 await _dbContext.SaveChangesAsync();
                 Utilisateur sender= _dbContext.Utilisateurs.Where(u => u.Id== senderId).FirstOrDefault();


            await Clients.User(receiver.ConnectionId).SendAsync("ReceiveMessage", sender.ConnectionId, message);
            await Clients.User(sender.ConnectionId).SendAsync("OwnMessage", message.Trim());
        }
        public async Task LibererConnectionId()
        {
            String Senderlogin = Context.GetHttpContext().Session.GetString("Login");
            Utilisateur sender = _dbContext.Utilisateurs.Where(u => u.Login == Senderlogin).FirstOrDefault();
            sender.ConnectionId = null;
            _dbContext.Utilisateurs.Update(sender);
            await _dbContext.SaveChangesAsync();

        }
    }


}
