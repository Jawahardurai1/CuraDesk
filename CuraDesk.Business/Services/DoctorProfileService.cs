using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CuraDesk.Business.Services
{
    public class DoctorProfileService : IDoctorProfile
    {
        private readonly IDoctorProfileRepository _repository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;
        public DoctorProfileService(IDoctorProfileRepository repository, IUserRepository userRepository,IDoctorAvailabilityRepository doctorAvailabilityRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _availabilityRepository = doctorAvailabilityRepository;

        }
        public async Task<DoctorProfileResponseDto?> CreateProfileAsync(Guid userId, CreateDoctorProfileDto dto)
        {
            Console.WriteLine("service reaxhecx");
            var user = await _userRepository.GetUserByIdAsync(userId);
            Console.WriteLine("afetr 1 condition reaxhecx");
            if (user == null || user.Role != "Doctor")
                return null;
            Console.WriteLine("after 2 nd con reaxhecx");


            var profile = new DoctorProfile
            {
                UserId = userId,
                Specialization = dto.Specialization,
                Qualification = dto.Qualification,
                YearsOfExperience = dto.YearsOfExperience,

            };
            Console.WriteLine("after creation reaxhecx");
            await _repository.AddAsync(profile);
            return await MapToDtoAsync(profile);
        }

        public async Task<List<DoctorProfileResponseDto?>> GetAllDoctorsAsync()
        {
            var profiles = await _repository.GetAllAsync();

            var result = new List<DoctorProfileResponseDto>();
            foreach (var profile in profiles)
                result.Add(await MapToDtoAsync(profile));

            return result;
        }
        public async Task<DoctorProfileResponseDto?> UpdateProfileAsync(Guid userId, UpdateDoctorProfileDto dto)
        {
            var profile = await _repository.GetByUserIdAsync(userId);
            if (profile == null)
                return null;

            profile.Specialization = dto.Specialization;
            profile.Qualification = dto.Qualification;
            profile.YearsOfExperience = dto.YearsOfExperience;
            
            await _repository.TaskSaveChanges();

            return await MapToDtoAsync(profile);
        }
        public async Task<DoctorProfileResponseDto?> GetProfileByUserIdAsync(Guid userId)
        {
            var profile = await _repository.GetByUserIdAsync(userId);

            if (profile == null)
                return null;

            return await MapToDtoAsync(profile);
        }

        private async Task<DoctorProfileResponseDto?> MapToDtoAsync(DoctorProfile profile)
        {
            var user = await _userRepository.GetUserByIdAsync(profile.UserId);
           
            return new DoctorProfileResponseDto
            {
                DoctorProfileId = profile.DoctorProfileId,
                UserId = profile.UserId,
                FullName = user?.UserName ?? string.Empty,
                Email = user?.EmailId ?? string.Empty,
                Specialization = profile.Specialization,
                Qualification = profile.Qualification,
                YearsOfExperience = profile.YearsOfExperience,
                

            };
        }
    }
}
