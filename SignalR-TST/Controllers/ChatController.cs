using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_TST.DTOs;
using SignalR_TST.Models;
using System.Security.Claims;

namespace SignalR_TST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ChatController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ChatMessageDTO>>> GetChatHistory(string userId, string receiverId)
        {
            var userclaim = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userclaim != null)
            {
                var user0 = userclaim.Value;
                var user = await _userManager.FindByNameAsync(user0);

                if (user == null || (user.Id != userId && user.Id != receiverId))
                {
                    return Unauthorized("User is not authorized to view this chat history.");
                }

                var chatHistory = await _context.ChatMessages
                    .Where(m => (m.SenderUserId == userId && m.ReceiverUserId == receiverId) ||
                                (m.SenderUserId == receiverId && m.ReceiverUserId == userId))
                    .OrderBy(m => m.Timestamp)
                    .Select(m => new ChatMessageDTO
                    {
                        SenderUserId = m.SenderUserId,
                        ReceiverUserId = m.ReceiverUserId,
                        Message = m.Message,
                        Timestamp = m.Timestamp
                    })
                    .ToListAsync();

                return Ok(chatHistory);
            }
            else
            {
                return Unauthorized("User is not authenticated.");
            }
        }

    }
}
