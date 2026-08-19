using CuraDesk.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CuraDesk.Business.Dto
{
    public class BookAppointmentDto
    {
        [Required]
        public Guid AvailabilityId { get; set; }
        [Required]
        [MinLength(3)]
        public string Notes { get; set; } = string.Empty;
    }
    public class AppointmentResponseDto
    {
        public Guid AppointmentId { get; set; }
        public Guid PatientUserId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Guid DoctorUserId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
       
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateAcceptResponseDto
    {
        public Guid AppointmentId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Accepted;
    }
    public class UpdateRejectResponseDto
    {
        public Guid AppointmentId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Declined;
    }

}
