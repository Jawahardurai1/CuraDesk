using CuraDesk.Business.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto?> BookAppointmentAsync(Guid patientUserId, BookAppointmentDto dto);
        Task<List<AppointmentResponseDto>> GetMyAppointmentsAsPatientAsync(Guid patientUserId);
        Task<List<AppointmentResponseDto>> GetMyAppointmentsAsDoctorAsync(Guid doctorUserId);
        Task<AppointmentResponseDto?> UpdateAcceptance(Guid AppointmentId);
        Task<UpdateRejectResponseDto?> UpdateRejected(Guid AppointmentId);
    }
}
