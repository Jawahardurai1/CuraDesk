using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class CreatePatientProfileDto
    {
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string ChronicConditions { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class UpdatePatientProfileDto : CreatePatientProfileDto { }
    public class PatientProfileResponseDto
    {
        public Guid PatientProfileId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string ChronicConditions { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
