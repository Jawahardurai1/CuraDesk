using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Repository
{
    public interface IMedicalReportRepository
    {
        Task<MedicalReport?> GetByIdAsync(Guid reportId);
        Task<List<MedicalReport>> GetByPatientIdAsync(Guid patientUserId, DateTime? date, string? diagnosisTag);
        Task AddAsync(MedicalReport report);
        Task SaveChangesAsync();
    }
}
