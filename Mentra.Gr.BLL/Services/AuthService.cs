using Mentra.Gr.BLL.Dtos;
using Mentra.Gr.BLL.Exceptions;
using Mentra.Gr.BLL.Interfaces;
using Mentra.Gr.BLL.Interfaces.Auth;
using Mentra.Gr.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IEmailService _emailService;
        private static Dictionary<string, string> _otpStore = new();


        public AuthService(UserManager<AppUser> userManager, IOptions<JwtOptions> options, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtOptions = options.Value;
            _emailService = emailService;

        }



        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }




        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            return result.Succeeded;
        }

        //  OTP 
        public async Task<bool> SendOtpAsync(EmailRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            var otp = new Random().Next(100000, 999999).ToString();
            _otpStore[request.Email] = otp;

            var body = $"<h2>Your OTP Code</h2><p>Your verification code is: <b>{otp}</b></p>";
            await _emailService.SendEmailAsync(request.Email, "Password Reset OTP", body);

            return true;
        }

        public Task<bool> VerifyOtpAsync(string email, string otp)
        {
            if (_otpStore.ContainsKey(email) && _otpStore[email] == otp)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }


        public async Task<UserResponse> LoginAsync(SignInDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new UserNotFoundException(request.Email);
            var flag = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!flag) throw new UnauthorizedException();
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }
        public async Task<UserResponse> RegisterAsync(SignUpDto request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                Email = request.Email,
                Age = request.Age
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegistrationBadRequsetException(result.Errors.Select(E => E.Description).ToList());
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {

            var authClaims = new List<Claim>() {

                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(

                  issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.Now.AddHours(_jwtOptions.DurtioninDays),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);





        }



    }
}
