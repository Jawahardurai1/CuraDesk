using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using System;
using CuraDesk.Model.Entities;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;
using CuraDesk.Utility.Email;

namespace CuraDesk.Business.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService emailService;
        public UserService( IUserRepository userRepository ,IEmailService email )
        {
            _userRepository = userRepository;
            emailService = email;
        }
        public async Task<UserResponseDto?> AddUserAsync(CreateUserDto userDto)
        {
            var ExistingUser=await  _userRepository.GetUserByEmailIdAsync(userDto.EmailId);
            if (ExistingUser != null) { return null; }

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                EmailId = userDto.EmailId,
                Role = userDto.Role,
            };
            await _userRepository.CreateUserAsync(user);
            await emailService.SendEmailAsync(user.EmailId, "CuraDesk - Temporary Password", $"Your temporary password is: {userDto.Password}" +
                $"Kindly Login through the temporary Password and change Password before Usage of the application "); ;
            return MaptoDto(user);

        }
        public static UserResponseDto MaptoDto(User user)
        {
            return new UserResponseDto
            {
                FullName = user.UserName,
                Email = user.EmailId,
                Role= user.Role,
                UserId = user.UserId,
                CreatedAt=user.CreatedAt
               
            };

        }
    }
}
