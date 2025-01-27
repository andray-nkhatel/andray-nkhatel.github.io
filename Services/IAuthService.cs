using Rentbook.DTOs;
using Rentbook.Models;

namespace Rentbook.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task<RegisterResult> Register(RegisterModel registerModel);
        Task<VerifyOtpResult> VerifyOtp(VerifyOtpModel model);
        Task<bool> ResendOtp(string email);
        Task Logout();
        Task<bool> IsUserAuthenticated();
    }
}
