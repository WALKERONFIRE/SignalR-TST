using SignalR_TST.DTOs;

namespace SignalR_TST.Interface
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(UserDTO model);
        Task<AuthModel> GetJwtToken(LoginDto model);
    }
}
