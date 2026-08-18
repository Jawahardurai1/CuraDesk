using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class CreateDoctorProfileDto
    {
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
    }
    public class UpdateDoctorProfileDto : CreateDoctorProfileDto { }
    public class DoctorProfileResponseDto
    {
        public Guid DoctorProfileId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public bool isBooked { get; set; }
     
    }
}
