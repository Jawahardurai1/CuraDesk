using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using CuraDesk.Exceptions;
using CuraDesk.Business.Exceptions;

namespace CuraDesk.Business.Services
{
   
    public class DoctorAvailabilityService:IDoctorAvailabilityService
    {
        private readonly IDoctorAvailabilityRepository doctorAvailabilityRepository;
        private readonly IUserRepository _userRepository;
        public DoctorAvailabilityService(IDoctorAvailabilityRepository _doctorAvailabilityRepository, IUserRepository userRepository    )
        {
            doctorAvailabilityRepository = _doctorAvailabilityRepository;
            _userRepository = userRepository;
        }

        public async Task<AvailabilityResponseDto?> AddAvailabilityAsync(Guid doctorUserId, CreateDoctorAvailabilityDto dto)
        {
            if(dto.EndTime<=dto.StartTime) { throw new TimeMissMatchException("Please Enter the Valid Time Line"); }
            var availability = new DoctorAvailability
            {
                DoctorUserId = doctorUserId,
                AvailableDate = dto.AvailableDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                isBooked = false
            };
            await doctorAvailabilityRepository.AddAsync(availability);
            return await MaptoDto(availability);
            
        }

        public async Task<List<AvailabilityResponseDto>> GetDoctorAvailabilityAsync(Guid doctorUserId)
        {
            var slots = await doctorAvailabilityRepository.GetByDoctorIdAsync(doctorUserId);
            var res = new List<AvailabilityResponseDto>();

            foreach (var slot in slots)
            {
                res.Add(await MaptoDto(slot));

            }
            return res;
        }

        private async Task<AvailabilityResponseDto> MaptoDto(DoctorAvailability availability)
        {
            var doc = await _userRepository.GetUserByIdAsync(availability.DoctorUserId);
            if (doc == null) { throw new NotFoundException("Doctor Not Founded!"); }
            return new AvailabilityResponseDto
            {
                AvailabilityId = availability.DoctorAvailabilityId,
                DoctorUserId = availability.DoctorUserId,
                AvailableDate = availability.AvailableDate,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                IsBooked = availability.isBooked,
                DoctorName = doc.UserName ?? string.Empty

            };
        }

    }
}
