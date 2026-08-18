using System;
using System.Collections.Generic;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class PassworResetDto
    {
        public string MailId { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set;  } = string.Empty;
    }

    public class ResponseDto
    {
        public string MailId { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
