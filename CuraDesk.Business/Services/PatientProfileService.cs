using System;
using System.Collections.Generic;
using System.Text;
using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Model.Entities;
namespace CuraDesk.Business.Services
{
    public class PatientProfileService:IPatientProfileService
    {
        private readonly IPatientProfileRepository repository;
        private readonly IUserRepository userRepository;
        public PatientProfileService(IPatientProfileRepository patientProfileRepository, IUserRepository userRepository )
        {
            repository = patientProfileRepository;
            userRepository = userRepository;
        }
        public async Task<PatientProfileResponseDto?> CreateProfileAsync(Guid userId, CreatePatientProfileDto dto)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if(user==null | user.Role!="Patient")
            {
                return null;
            }
            var ExistingProfile=await repository.GetPatientProfileAsync(userId);
            if(ExistingProfile!=null) { return null; }


            var profile = new PatientProfile
            {
                UserId = userId,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                BloodGroup = dto.BloodGroup,
                Address = dto.Address,
                Allergies = dto.Allergies,
                ChronicConditions = dto.ChronicConditions,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone,
            };
            await repository.AddAsync(profile);
            return await MapToDtoAsync(profile);
        }

        public async Task<PatientProfileResponseDto?> GetProfileByUserIdAsync(Guid userId)
        {
            var profile = await repository.GetPatientProfileAsync(userId);
            return profile==null ?null :await MapToDtoAsync(profile);
        }

        public async Task<PatientProfileResponseDto?> UpdateProfileAsync(Guid userId, UpdatePatientProfileDto dto)
        {
            var profile = await repository.GetPatientProfileAsync(userId);
            if(profile==null) { return null; }
            
            profile.EmergencyContactPhone = dto.EmergencyContactPhone;
            profile.EmergencyContactName = dto.EmergencyContactName;
            profile.BloodGroup= dto.BloodGroup;
            profile.Allergies = dto.Allergies;
            profile.ChronicConditions = dto.ChronicConditions;
            profile.EmergencyContactName = dto.EmergencyContactName;
            profile.EmergencyContactPhone = dto.EmergencyContactPhone;
            profile.Address = dto.Address;

            await repository.SaveChangesAsync();
            return await MapToDtoAsync(profile);


        }


        private async Task<PatientProfileResponseDto?>MapToDtoAsync(PatientProfile profile)
        {
            var user=await userRepository.GetUserByIdAsync(profile.UserId);
            return new PatientProfileResponseDto
            {
                PatientProfileId = profile.PatientId,
                UserId = profile.UserId,
                FullName = user.UserName,
                Email = user.EmailId,
                DateOfBirth = profile.DateOfBirth,
                Gender = profile.Gender,
                BloodGroup = profile.BloodGroup,
                Address = profile.Address,
                Allergies = profile.Allergies,
                ChronicConditions = profile.ChronicConditions,
                EmergencyContactName = profile.EmergencyContactName,
                EmergencyContactPhone = profile.EmergencyContactPhone,

            };
        }
    }
}
