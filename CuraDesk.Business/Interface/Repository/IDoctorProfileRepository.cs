using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Repository
{
    public interface IDoctorProfileRepository
    {
        Task<DoctorProfile?> GetByUserIdAsync(Guid userId);
        Task<List<DoctorProfile>> GetAllAsync();
        Task AddAsync(DoctorProfile profile);
        Task TaskSaveChanges();
    }
}
