using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using SignalR_TST.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_TST
{
    public sealed class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;



        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<string> GetCurrentUserAsync()
        {
            var user1 =  _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return user1;
        }
        public async Task SendMessageAsync(string Sender, string Receiver, string message)
        {
            var chatMessage = new ChatMessage
            {
                SenderUserId = Sender,
                ReceiverUserId = Receiver,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.User(Receiver).SendAsync("ReceiveMessage", Sender, message);
        }

        public override async Task OnConnectedAsync()
        {

            var user1 = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByNameAsync(user1);
            if (user != null)
            {

                await Clients.All.SendAsync("ReceiveMessage", $"{user.Name} ({Context.ConnectionId}) Is ONLINE!");

            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} Is ONLINE!");
            }


            var connection = new Connection
            {
                ConId = Context.ConnectionId,
                UserId = user.Id,
            };
            await _context.Connections.AddAsync(connection);
            _context.SaveChanges();
        }
        //public override async Task OnDisconnectedAsync()
        //{

        //}

    }
}
