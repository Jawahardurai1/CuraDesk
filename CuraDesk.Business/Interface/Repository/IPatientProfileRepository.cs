using CuraDesk.Business.Dto;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Repository
{
    public interface IPatientProfileRepository
    {
        Task<PatientProfile?> GetPatientProfileAsync(Guid id);
        Task AddAsync(PatientProfile profile);

        Task SaveChangesAsync();
    }
}
