using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Data.Repositories
{
   
    public class DoctorProfileRepository:IDoctorProfileRepository
    {
        private readonly AppDbContext _appDbcontext;
        public DoctorProfileRepository(AppDbContext appDbcontext)
        {
            _appDbcontext = appDbcontext;
        }

        public async Task<DoctorProfile?> GetByUserIdAsync(Guid userId)
        {
            return await _appDbcontext.DoctorProfiles.FirstOrDefaultAsync(d=>d.UserId==userId);
        }
        public async Task<List<DoctorProfile>> GetAllAsync()
        {
           return await _appDbcontext.DoctorProfiles.ToListAsync();
        }
        public async Task AddAsync(DoctorProfile profile)
        {
            await _appDbcontext.DoctorProfiles.AddAsync(profile);
            await _appDbcontext.SaveChangesAsync();
        }
        public async Task TaskSaveChanges()
        {
            await _appDbcontext.SaveChangesAsync();
        }
    }
}
