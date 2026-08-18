using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set;  } = string.Empty;
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
