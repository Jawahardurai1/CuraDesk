using CuraDesk.Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Service
{
    public interface IUserService
    {
        Task<UserResponseDto?> AddUserAsync(CreateUserDto userDto);

       
    }
}
