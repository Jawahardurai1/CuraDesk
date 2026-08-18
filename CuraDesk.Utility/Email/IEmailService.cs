using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Utility.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string receiverEmail,
            string subject,
            string message);
    }
}
