using CuraDesk.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CuraDesk.Model.Entities
{
    public class Appointments
    {
        [Key]
        public Guid AppointmentId { get; set; } = Guid.NewGuid();
        public Guid PatientUserId { get; set; }
        public Guid DoctorId  { get; set; }
        public Guid AvailabilityId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }= AppointmentStatus.Requested;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string Notes { get; set; }=string.Empty;
        public DoctorAvailability? Availability { get; set; }
        public User? Patient { get; set; }
        public User? Doctor { get; set; }


    }

   

   
}
