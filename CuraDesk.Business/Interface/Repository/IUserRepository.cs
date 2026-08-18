using CuraDesk.Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using CuraDesk.Model.Entities;
namespace CuraDesk.Business.Interface.Repository
{
    public  interface IUserRepository
    {
         Task<User?> CreateUserAsync(User user);
        Task<User?>GetUserByEmailIdAsync(string Eid);
        Task<User?> GetUserByIdAsync(Guid Id);
        Task<bool> UpdatePasswordAsync(Guid userId, string passwordHash);
    }
}
