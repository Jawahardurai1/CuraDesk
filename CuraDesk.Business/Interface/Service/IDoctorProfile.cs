using CuraDesk.Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Service
{
    public interface IDoctorProfile
    {
        Task<DoctorProfileResponseDto?> CreateProfileAsync(Guid userId, CreateDoctorProfileDto dto);
        Task<DoctorProfileResponseDto?> GetProfileByUserIdAsync(Guid userId);

        Task<List<DoctorProfileResponseDto?>> GetAllDoctorsAsync();
        Task<DoctorProfileResponseDto?> UpdateProfileAsync(Guid userId, UpdateDoctorProfileDto dto);
    }
}
