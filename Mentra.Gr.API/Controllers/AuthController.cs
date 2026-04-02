using Mentra.Gr.BLL.Dtos;
using Mentra.Gr.BLL.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentra.Gr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> login(SignInDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }


        [HttpGet("EmailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await _authService.CheckEmailExistAsync(email);
            return Ok(result);
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailRequestDto request)
        {
            var sent = await _authService.SendOtpAsync(request);
            if (!sent) return BadRequest("Email not found");

            return Ok("OTP sent");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var ok = await _authService.VerifyOtpAsync(dto.Email, dto.Otp);

            if (!ok) return BadRequest("Invalid OTP");

            return Ok("OTP Verified");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _authService.ResetPasswordAsync(model.Email, model.NewPassword);

            if (!result)
                return BadRequest(new { message = "Failed to reset password" });

            return Ok(new { message = "Password reset successfully" });
        }


    }
}
