using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CuraDesk.Business.Dto
{
    public class PassworResetDto
    {
        [Required]
        [EmailAddress]
        public string MailId { get; set; } = string.Empty;
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        [MinLength(7)]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set;  } = string.Empty;
    }

    public class ResponseDto
    {
        public string MailId { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
