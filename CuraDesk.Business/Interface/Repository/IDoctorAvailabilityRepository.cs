using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Repository
{
    public interface IDoctorAvailabilityRepository
    {
        Task<DoctorAvailability?> GetByIdAsync(Guid AvailabilityId);
        Task<List<DoctorAvailability>> GetByDoctorIdAsync(Guid doctorUserId);
        Task AddAsync(DoctorAvailability availability);
    }
}
