using Mentra.Gr.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<UserResponse> LoginAsync(SignInDto request);
        Task<UserResponse> RegisterAsync(SignUpDto request);

        Task<bool> CheckEmailExistAsync(string email);

        Task<bool> SendOtpAsync(EmailRequestDto request);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }
}
