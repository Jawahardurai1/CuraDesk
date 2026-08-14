using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CuraDesk.Model.Entities
{
    public class DoctorAvailability
    {
        [Key]
        public Guid DoctorAvailabilityId { get; set; } = Guid.NewGuid();
        public Guid DoctorUserId { get; set; }
        public DateTime AvailableDate { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public bool isBooked { get; set; }=false;
        public User? Doctor { get; set; }

    }
}
