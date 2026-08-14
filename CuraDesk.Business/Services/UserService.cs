using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using System;
using CuraDesk.Model.Entities;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace CuraDesk.Business.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService( IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserResponseDto?> AddUserAsync(CreateUserDto userDto)
        {
            var ExistingUser=await  _userRepository.GetUserByIdAsync(userDto.EmailId);
            if (ExistingUser != null) { return null; }

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                EmailId = userDto.EmailId,
                Role = userDto.Role,
            };
            await _userRepository.CreateUserAsync(user);
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
