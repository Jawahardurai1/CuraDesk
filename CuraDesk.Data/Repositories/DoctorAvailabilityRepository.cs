using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Data.Repositories
{
    public class DoctorAvailabilityRepository:IDoctorAvailabilityRepository
    {
        private readonly AppDbContext appDbContext;
        public DoctorAvailabilityRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;

        }

        public async Task<DoctorAvailability?> GetByIdAsync(Guid AvailabilityId)
        {
            return await appDbContext.DoctorAvailability.FindAsync(AvailabilityId);
        }
        public async Task<List<DoctorAvailability>> GetByDoctorIdAsync(Guid doctorUserId)
        {
            return await appDbContext.DoctorAvailability.Where(d => d.DoctorUserId == doctorUserId && !d.isBooked).OrderBy(d => d.AvailableDate).ThenBy(d => d.StartTime).ToListAsync();
        }

        public async Task  AddAsync(DoctorAvailability availability)
        {
             await appDbContext.DoctorAvailability.AddAsync(availability);
          await  appDbContext.SaveChangesAsync();
         
        }



    }

}
