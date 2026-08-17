using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CuraDesk.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbcontext;
        public UserRepository(AppDbContext appDbcontext)
        {
            _dbcontext = appDbcontext;

        }
        public async Task<User?> CreateUserAsync(User user)
        {

            await _dbcontext.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            return user;


        }
        public async Task<User?> GetUserByEmailIdAsync(string Eid)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(e => e.EmailId == Eid);
            if (user == null) return null;
            return user;

        }
       public async Task<User?> GetUserByIdAsync(Guid Id)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(e => e.UserId == Id);
            if (user == null) return null;
            return user;
        }
    }
}
