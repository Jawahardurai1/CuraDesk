using CuraDesk.Business.Dto;
using CuraDesk.Business.Exceptions;
using CuraDesk.Business.Interface;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Exceptions;
using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
namespace CuraDesk.Business.Services
{
    public class PatientProfileService:IPatientProfileService
    {
        private readonly IPatientProfileRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IAppointmentRepository appointmentRepository;
        public PatientProfileService(IPatientProfileRepository patientProfileRepository, IUserRepository _userRepository ,IAppointmentRepository _appointmentRepository)
        {
            repository = patientProfileRepository;
            userRepository = _userRepository;
            appointmentRepository = _appointmentRepository;
        }
        public async Task<PatientProfileResponseDto?> CreateProfileAsync(Guid userId, CreatePatientProfileDto dto)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if(user==null || user.Role!="Patient")
            {
                throw new NotFoundException("User nor found or Authorization for this role is denied"); 
            }
            var ExistingProfile=await repository.GetPatientProfileAsync(userId);
            if(ExistingProfile!=null) { throw new AlreadyExistsException("The Profile already Exists"); }


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
            if(profile==null) { throw new NotFoundException("Profile Not Found Execption "); }
            
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
        public async Task<PatientProfileResponseDto?> GetProfileForDoctorAsync(Guid doctorUserId, Guid patientUserId)
        {
            bool hasAppointment = await appointmentRepository.DoctorHasAppointmentWithPatientAsync(doctorUserId, patientUserId);
            if (!hasAppointment)
                 throw new NotFoundException("Doctor has no appointment with this patient "); ;  

            var profile = await repository.GetPatientProfileAsync(patientUserId);
            return profile == null ? null : await MapToDtoAsync(profile);

        }

        private async Task<PatientProfileResponseDto?>MapToDtoAsync(PatientProfile profile)
        {
            var user=await userRepository.GetUserByIdAsync(profile.UserId);
            return new PatientProfileResponseDto
            {
                PatientProfileId = profile.PatientId,
                UserId = profile.UserId,
                FullName = user?.UserName ?? string.Empty,
                Email = user?.EmailId ?? string.Empty,
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
