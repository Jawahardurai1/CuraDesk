using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CuraDesk.Model.Entities
{
    public class PatientProfile
    {
        [Key]
       public Guid PatientId { get; set; }= Guid.NewGuid();
       public Guid UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public string Gender { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string ChronicConditions { get; set; } = string.Empty;  
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
   
        public User? User { get; set; }
    }
}
