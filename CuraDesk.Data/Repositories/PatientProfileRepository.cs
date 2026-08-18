using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Data.Repositories
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        private readonly AppDbContext _appDbContext;
        public PatientProfileRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public async Task<PatientProfile?> GetPatientProfileAsync(Guid UserId)
        {
            return await _appDbContext.PatientProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == UserId);
        }
        public async Task AddAsync(PatientProfile profile)
        {
            await _appDbContext.PatientProfiles.AddAsync(profile);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
