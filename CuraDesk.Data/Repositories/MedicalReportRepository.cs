using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Data.Repositories
{
    public class MedicalReportRepository : IMedicalReportRepository
    {
        private readonly AppDbContext appDbcontext;
        public MedicalReportRepository(AppDbContext _appDbcontext)
        {
            appDbcontext = _appDbcontext;
        }
        public async Task<MedicalReport?> GetByIdAsync(Guid reportId)
        {
           return await appDbcontext.MedicalReports.FindAsync(reportId);
        }

        public async Task<List<MedicalReport>> GetByPatientIdAsync(Guid patientUserId, DateTime? date, string? diagnosisTag)
        {
            var query = appDbcontext.MedicalReports.Where(r => r.PatientUserId == patientUserId);

            if (date.HasValue)
                query = query.Where(r => r.ReportDate.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(diagnosisTag))
                query = query.Where(r => r.DiagnosisTag.Contains(diagnosisTag));

            return await query.OrderByDescending(r => r.UploadedAt).ToListAsync();
        }
        public async Task AddAsync(MedicalReport report) =>
           await appDbcontext.MedicalReports.AddAsync(report);

        public async Task SaveChangesAsync() =>
            await appDbcontext.SaveChangesAsync();
    }
}
