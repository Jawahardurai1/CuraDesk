using CuraDesk.Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Service
{
    public interface IPatientProfileService
    {
        Task<PatientProfileResponseDto?> CreateProfileAsync(Guid userId, CreatePatientProfileDto dto);
        Task<PatientProfileResponseDto?> GetProfileByUserIdAsync(Guid userId);
        Task<PatientProfileResponseDto?> UpdateProfileAsync(Guid userId, UpdatePatientProfileDto dto);
        Task<PatientProfileResponseDto?> GetProfileForDoctorAsync(Guid doctorUserId, Guid patientUserId);
    }
}
