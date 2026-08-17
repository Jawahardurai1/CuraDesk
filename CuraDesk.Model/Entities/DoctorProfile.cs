using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CuraDesk.Model.Entities
{
    public class DoctorProfile
    {
        [Key]
        public Guid DoctorProfileId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; } 

        public string Specialization { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }
       
        public User? User { get; set; }

    }
}
