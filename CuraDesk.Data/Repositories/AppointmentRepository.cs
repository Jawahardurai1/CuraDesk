using CuraDesk.Business.Interface.Repository;
using CuraDesk.Data.Context;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointments?> GetByIdAsync(Guid appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Availability)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }
        public async Task<List<Appointments>> GetByPatientIdAsync(Guid patientUserId)
        {
            return await _context.Appointments.Where(a => a.PatientUserId == patientUserId).OrderByDescending(a => a.AppointmentDate).ToListAsync();
        }

        public async Task<List<Appointments>> GetByDoctorIdAsync(Guid doctorUserId)
        {
            return await _context.Appointments.Where(a => a.DoctorId == doctorUserId).OrderByDescending(a => a.AppointmentDate).ToListAsync();
        }

        public async Task<bool> DoctorHasAppointmentWithPatientAsync(Guid doctorUserId, Guid patientUserId)
        {
            return await _context.Appointments
                  .AnyAsync(a => a.DoctorId == doctorUserId && a.PatientUserId == patientUserId);
        }
        public async Task AddAsync(Appointments appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
