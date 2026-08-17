using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using Microsoft.Extensions.Configuration;
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

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailIdAsync(dto.Email);

            if (user == null) { return null; }

            bool ValidatePassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!ValidatePassword) { return null; }

            var token = GenerateJwtToken(user.UserId, user.EmailId, user.Role);
            return new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                FullName = user.UserName,
                Role = user.Role,

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
