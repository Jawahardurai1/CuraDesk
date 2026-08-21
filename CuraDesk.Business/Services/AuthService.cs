using CuraDesk.Business.Dto;
using CuraDesk.Business.Exceptions;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Exceptions;
using CuraDesk.Utility.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CuraDesk.Business.Services
{

    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUserRepository userRepository, IConfiguration config,IEmailService email,ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _config = config;
            _emailService= email;
            _logger = logger;
        }
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailIdAsync(dto.Email);

            if (user == null) {
                _logger.LogError("User Doesnt Found");
                throw new NotFoundException("User not found"); }
           

            bool ValidatePassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!ValidatePassword) { throw new PasswordMisMatchException("Verify the Entered Password"); }
            if (!user.isFirstLogin)
            {
                return new AuthResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.UserName,
                    Role = user.Role,
                    RequirePasswordChange = true,
                    Message = "Please change your temporary password"
                };
            }

            var token = GenerateJwtToken(user.UserId, user.EmailId, user.Role);
            return new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                FullName = user.UserName,
                Role = user.Role,

            };

        }

        public async Task<ResponseDto?> ResetPasswordAsync(PassworResetDto dto)
        {
            var user = await _userRepository.GetUserByEmailIdAsync(dto.MailId);
            if (user == null) { throw new NotFoundException($"User not found"); }

            if(dto.NewPassword!=dto.ConfirmPassword) { throw new PasswordMisMatchException($"Verify the entered password"); }

            bool ValidatePassword = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash);
            if (!ValidatePassword) { throw new PasswordMisMatchException($"Verify the entered password"); }

            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            bool updatepass =await  _userRepository.UpdatePasswordAsync(user.UserId, newPasswordHash);
            if (!updatepass) { throw new NotFoundException($"User not found"); }

            await _emailService.SendEmailAsync(user.EmailId, "CuraDesk - Reset Password Successful","Password Changed Sucessfully!");
            return new ResponseDto
            {
                MailId = dto.MailId,
                NewPassword = dto.NewPassword,
            };

        }

        private string GenerateJwtToken(Guid UserId, string Email, string role)
        {

            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, UserId.ToString()),
                    new(ClaimTypes.Email, Email),
                    new(ClaimTypes.Role, role)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
