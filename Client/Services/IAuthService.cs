using ControleVendas.Shared.Models;

namespace ControleVendas.Client.Services
{ 
    public record AuthResponse(bool Success, string Message, string Token);

    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginModel loginModel);
        Task<AuthResponse> Register(LoginModel registerModel);
        Task Logout();
    }
}
