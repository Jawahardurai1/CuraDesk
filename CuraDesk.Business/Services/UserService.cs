using BCrypt.Net;
using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Exceptions;
using CuraDesk.Model.Entities;
using CuraDesk.Utility.Email;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (ExistingUser != null) { throw new NotFoundException("A user with this email already exists. "); }

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                EmailId = userDto.EmailId,
                Role = userDto.Role,
                PhoneNumber= userDto.PhoneNumber,
            };
            await _userRepository.CreateUserAsync(user);

            await emailService.SendEmailAsync(user.EmailId, "CuraDesk - Temporary Password", $"Your temporary password is  : {userDto.Password}" +
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
                PhoneNumber= user.PhoneNumber,
                UserId = user.UserId,
                CreatedAt=user.CreatedAt
               
            };

        }
    }
}
