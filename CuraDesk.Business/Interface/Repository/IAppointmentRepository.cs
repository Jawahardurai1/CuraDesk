using CuraDesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Interface.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointments?> GetByIdAsync(Guid appointmentId);
        Task<List<Appointments>> GetByPatientIdAsync(Guid patientUserId);
        Task<List<Appointments>> GetByDoctorIdAsync(Guid doctorUserId);
        Task<bool> DoctorHasAppointmentWithPatientAsync(Guid doctorUserId, Guid patientUserId);
        Task AddAsync(Appointments appointment);
        Task SaveChangesAsync();
    }
}
