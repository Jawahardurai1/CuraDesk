using CuraDesk.Model.DTOs;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Service
{
    public interface IMedicalReportService
    {
        Task<MedicalReportResponseDto> UploadReportAsync(Guid patientUserId, string fileName, string fileUrl, UploadReportMetadataDto dto);
        Task<List<MedicalReportResponseDto>> GetMyReportsAsync(Guid patientUserId, DateTime? date, string? diagnosisTag);
        Task<MedicalReport?> GetReportForAccessCheckAsync(Guid requesterId, string requesterRole, Guid reportId);
        Task<List<MedicalReportResponseDto>?> GetPatientReportsForDoctorAsync(Guid doctorUserId, Guid patientUserId);
    }
}
