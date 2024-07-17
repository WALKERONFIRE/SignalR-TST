using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using SignalR_TST.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_TST.DTOs;

namespace SignalR_TST
{
    public sealed class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ChatHub> _logger;



        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, ILogger<ChatHub> logger)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }
        public async Task<string> GetCurrentUserAsync()
        {
            var user1 =  _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return user1;
        }
        public async Task SendMessageAsync(ChatMessageDTO dto)
        {
             var message = new ChatMessage
            {
                SenderUserId = dto.SenderUserId,
                ReceiverUserId = dto.ReceiverUserId,
                Message = dto.Message,
                Timestamp = DateTime.UtcNow
            };

            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();

            await Clients.User(message.ReceiverUserId).SendAsync("ReceiveMessage", dto);
        }

        public override async Task OnConnectedAsync()
        {

            var user1 = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByNameAsync(user1);
            if (user != null)
            {
                var connection = new Connection
                {
                    ConId = Context.ConnectionId,
                    UserId = user.Id,
                };
                await _context.Connections.AddAsync(connection);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveMessage", $"{user.Name} ({Context.ConnectionId}) Is ONLINE!");

            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} Is ONLINE!");
            }


            
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var connectionId = Context.ConnectionId;
                var connection = await _context.Connections.FirstOrDefaultAsync(c => c.ConId == connectionId);
                if (connection != null)
                {
                    _context.Connections.Remove(connection);
                    await _context.SaveChangesAsync();
                    await Clients.All.SendAsync("ReceiveMessage", $"{connectionId} is OFFLINE!");
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync");
                throw;
            }
        }

    }
}
