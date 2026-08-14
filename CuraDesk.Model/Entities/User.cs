using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Model.Entities
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string EmailId { get; set; } = "";
        public string Role { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
