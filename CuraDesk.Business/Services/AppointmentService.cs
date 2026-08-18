using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Model.Entities;
using CuraDesk.Model.Enums;
using CuraDesk.Utility.Email;
using CuraDesk.Utility.Voice;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CuraDesk.Business.Services
{
    public class AppointmentService:IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly IEmailService _emailService;
        private readonly IVoiceNotificationService _voiceNotificationService;
        public AppointmentService(
           IAppointmentRepository appointmentRepository,
           IDoctorAvailabilityRepository availabilityRepository,
           IEmailService emailService,
           IUserRepository userRepository,
           IVoiceNotificationService voiceNotificationService,
           IPatientProfileRepository patientProfileRepository)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
            _voiceNotificationService = voiceNotificationService;
            _patientProfileRepository = patientProfileRepository;
            _emailService = emailService;
        }
        public async Task<AppointmentResponseDto?> BookAppointmentAsync(Guid patientUserId, BookAppointmentDto dto)
        {
            var profile = await _patientProfileRepository.GetPatientProfileAsync(patientUserId);
            if (profile == null)
                throw new Exception($"Patient profile not found for UserId: {patientUserId}");

            var slot = await _availabilityRepository.GetByIdAsync(dto.AvailabilityId);

            if (slot == null)
                throw new Exception($"Availability slot not found for ID: {dto.AvailabilityId}");

            if (slot.isBooked)
                throw new Exception($"Availability slot {dto.AvailabilityId} is already booked");
            var appointment = new Appointments
            {
                PatientUserId = patientUserId,
                DoctorId = slot.DoctorUserId,
                AvailabilityId = slot.DoctorAvailabilityId,
                AppointmentDate = slot.AvailableDate,
               Status=AppointmentStatus.Requested,
                Notes = dto.Notes
            };

            slot.isBooked = true;
            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
            if (profile.User == null)
                throw new Exception("Patient profile exists, but User is null.");

            await _emailService.SendEmailAsync(
                profile.User.EmailId,
                "CuraDesk-Appointment Request",
                "Your Booking Appointment was Requested Successfully. Kindly check your email for further assistance and confirmation of the Appointment."
            );
            return await MapToDtoAsync(appointment, slot.StartTime);
        }
        public async Task<List<AppointmentResponseDto>> GetMyAppointmentsAsPatientAsync(Guid patientUserId)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientUserId);
            var result = new List<AppointmentResponseDto>();

            foreach (var appt in appointments)
            {
                var slot = await _availabilityRepository.GetByIdAsync(appt.AvailabilityId);
                result.Add(await MapToDtoAsync(appt, slot?.StartTime ?? TimeSpan.Zero));
            }

            return result;
        }

        public async Task<AppointmentResponseDto?> UpdateAcceptance(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception($"Appointment not found: {appointmentId}");

            if (appointment.Status != AppointmentStatus.Requested)
                throw new Exception(
              $"Appointment status is {appointment.Status}, expected Requested.");
            if (appointment.Patient == null)
                throw new Exception("Appointment.Patient is null.");

            if (appointment.Doctor == null)
                throw new Exception("Appointment.Doctor is null.");

            if (appointment.Availability == null)
                throw new Exception("Appointment.Availability is null.");

            if (appointment.Patient.PhoneNumber == null)
                throw new Exception("Patient phone number is null.");

            if (appointment.Doctor.UserName == null)
                throw new Exception("Doctor username is null.");

            appointment.Status = AppointmentStatus.Accepted;

            await _appointmentRepository.SaveChangesAsync();

            await _voiceNotificationService.SendAppointmentConfirmationCallAsync(
                appointment.Patient.PhoneNumber,
                appointment.Doctor.UserName,
                appointment.AppointmentDate.ToString("dd-MM-yyyy"),
                appointment.Availability.StartTime.ToString(@"hh\:mm")
            );

            await _emailService.SendEmailAsync(
                appointment.Patient.EmailId,
                "CuraDesk-Appointment Confirmation",
                "Your appointment has been accepted successfully."
            );

            return await MapToDtoAsync(
                appointment,
                appointment.Availability.StartTime
            );
        }
        public async Task<UpdateRejectResponseDto?> UpdateRejected(Guid AppointmentId)
        {
            var appointments = await _appointmentRepository.GetByIdAsync(AppointmentId);
            if (appointments == null) { return null; }

            if (appointments.Status != AppointmentStatus.Requested)
                return null;
            appointments.Status = AppointmentStatus.Declined;
            await _appointmentRepository.SaveChangesAsync();

            var slots = await _availabilityRepository.GetByIdAsync(appointments.AvailabilityId);
          
            await _appointmentRepository.AddAsync(appointments);
            await _appointmentRepository.SaveChangesAsync();
            
            await _emailService.SendEmailAsync(appointments.Patient.EmailId, "CuraDesk-Appointment Follow up", "Your appointment request has been rejected. You will receive an email with the appointment details and rejection information shortly.");
            return await MapToDtoRejectAsync(appointments);



        }
        private async Task<UpdateRejectResponseDto> MapToDtoRejectAsync(Appointments appointments)
        {
            return new UpdateRejectResponseDto
            {
                AppointmentId= appointments.AppointmentId,
                Status= appointments.Status,
            };
        }
        public async Task<List<AppointmentResponseDto>> GetMyAppointmentsAsDoctorAsync(Guid doctorUserId)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorUserId);
            var result = new List<AppointmentResponseDto>();

            foreach (var appt in appointments)
            {
                var slot = await _availabilityRepository.GetByIdAsync(appt.AvailabilityId);
                result.Add(await MapToDtoAsync(appt, slot?.StartTime ?? TimeSpan.Zero));
            }

            return result;
        }

        private async Task<AppointmentResponseDto?> MapToDtoAsync(Appointments appt, TimeSpan startTime)
        {
            var patient = await _userRepository.GetUserByIdAsync(appt.PatientUserId);
            var doctor = await _userRepository.GetUserByIdAsync(appt.DoctorId);
            return new AppointmentResponseDto
            {
                AppointmentId = appt.AppointmentId,
                PatientUserId = appt.PatientUserId,
                PatientName = patient?.UserName ?? string.Empty,
                DoctorUserId = appt.DoctorId,
                DoctorName = doctor?.UserName ?? string.Empty,
                AppointmentDate = appt.AppointmentDate,
                StartTime = startTime,
                
                Notes = appt.Notes,
                CreatedAt = appt.CreatedAt
            };
        }
    }
}
