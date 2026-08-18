using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public  class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
    public class AuthResponseDto
    {
        public string? Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool RequirePasswordChange { get; set; }
        public string? Message { get; set; }
    }

    public class FailsResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool RequirePasswordChange { get; set; }
    }
        
}
