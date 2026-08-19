using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class CreateDoctorAvailabilityDto
    {
        [Required]
        public DateTime AvailableDate { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
    }
    public class AvailabilityResponseDto
    {
        public Guid AvailabilityId { get; set; }
        public Guid DoctorUserId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
    }
}
