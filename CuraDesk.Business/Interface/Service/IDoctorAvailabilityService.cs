using System;
using System.Collections.Generic;
using System.Text;
using CuraDesk.Business.Dto;
namespace CuraDesk.Business.Interface.Service
{
    public interface IDoctorAvailabilityService
    {
        Task<AvailabilityResponseDto?> AddAvailabilityAsync(Guid doctorUserId, CreateDoctorAvailabilityDto dto);
        Task<List<AvailabilityResponseDto>> GetDoctorAvailabilityAsync(Guid doctorUserId);
    }
}
