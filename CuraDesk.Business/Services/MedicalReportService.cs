using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Model.DTOs;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Services
{
    public class MedicalReportService : IMedicalReportService
    {
        private readonly IMedicalReportRepository _reportRepository;
        private readonly IAppointmentRepository _appointmentReposity;
        public MedicalReportService(IMedicalReportRepository medicalReportRepository, IAppointmentRepository appointmentReposity)
        {
            _reportRepository = medicalReportRepository;
            _appointmentReposity = appointmentReposity;
        }
        public async Task<MedicalReportResponseDto> UploadReportAsync(Guid patientUserId, string fileName, string fileUrl, UploadReportMetadataDto dto)
        {
            var report = new MedicalReport
            {
                PatientUserId = patientUserId,
                FileName = fileName,
                FilePath = fileUrl,
                AppointmentId = dto.AppointmentId,
                DiagnosisTag = dto.DiagnosisTag,
                ReportDate = dto.ReportDate
            };
            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();

            return MapToDto(report);

        }
        public async Task<List<MedicalReportResponseDto>> GetMyReportsAsync(Guid patientUserId, DateTime? date, string? diagnosisTag)
        {
            var reports = await _reportRepository.GetByPatientIdAsync(patientUserId, date, diagnosisTag);
            return reports.Select(MapToDto).ToList();
        }

        public async Task<MedicalReport?> GetReportForAccessCheckAsync(Guid requesterId, string requesterRole, Guid reportId)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null) {  return null; }
            if(requesterRole=="Patient" && requesterId!=report.PatientUserId) { return null; }
            if(requesterRole=="Doctor")
            {
                bool AlreadyAppointed = await _appointmentReposity.DoctorHasAppointmentWithPatientAsync(requesterId, report.PatientUserId);
                if(AlreadyAppointed) { return null; }
            }
            return report;
        }

        public async Task<List<MedicalReportResponseDto>?> GetPatientReportsForDoctorAsync(Guid doctorUserId, Guid patientUserId)
        {
            bool AlreadyAppointed = await _appointmentReposity.DoctorHasAppointmentWithPatientAsync(doctorUserId,patientUserId);
            if (AlreadyAppointed) { return null; }
            var reports = await _reportRepository.GetByPatientIdAsync(patientUserId, null, null);
            return reports.Select(MapToDto).ToList();
        }

        private static MedicalReportResponseDto MapToDto(MedicalReport r)
        {
            return new MedicalReportResponseDto
            {
                ReportId = r.MReportId,
                PatientUserId = r.PatientUserId,
                FileName = r.FileName,
                FileUrl = r.FilePath,
                DiagnosisTag = r.DiagnosisTag,
                ReportDate = r.ReportDate,
                UploadedAt = r.UploadedAt
            };

        }
    }
}
