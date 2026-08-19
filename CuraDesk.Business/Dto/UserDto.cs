using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class CreateUserDto
    {
        [Required]

        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string EmailId { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Role { get; set;  } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;


    }

    public class UserResponseDto
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;


    }
}
