using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR_TST.DTOs;
using SignalR_TST.Interface;

namespace SignalR_TST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IAuthService authService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(_context.Users.ToList());
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Massage);
            }
            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.GetJwtToken(login);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Massage);
            }
            return Ok(result);
        }
    }
}
