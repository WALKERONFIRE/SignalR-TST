using Microsoft.AspNetCore.SignalR;
using SignalR_TST.Models;

namespace SignalR_TST
{
    public sealed class ChatHub : Hub
    {
        //private readonly ApplicationDbContext _context;

        //public ChatHub(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task SendMessageAsync(string Sender, string Receiver, string message)
        //{
        //    var chatMessage = new ChatMessage
        //    {
        //        SenderUserId = Sender,
        //        ReceiverUserId = Receiver,
        //        Message = message,
        //        Timestamp = DateTime.UtcNow
        //    };

        //    await _context.ChatMessages.AddAsync(chatMessage);
        //    await _context.SaveChangesAsync();

        //    await Clients.User(Receiver).SendAsync("ReceiveMessage", Sender, message);
        //}

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} Is ONLINE!");
        }
    }
}
