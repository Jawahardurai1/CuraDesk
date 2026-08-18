using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using CuraDesk.Utility.Email;
namespace CuraDesk.Utility.Email
{
   
     public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;
        }

        public async Task SendEmailAsync(
     string receiverEmail,
     string subject,
     string message)
        {
            try
            {
                Console.WriteLine("Email method reached");

                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

                email.To.Add(MailboxAddress.Parse(receiverEmail));
                email.Subject = subject;

                email.Body = new BodyBuilder
                {
                    HtmlBody = message
                }.ToMessageBody();

                using var smtpClient = new SmtpClient();

                Console.WriteLine("Connecting to SMTP...");

                await smtpClient.ConnectAsync(
                    _emailSettings.Host,
                    _emailSettings.Port,
                    SecureSocketOptions.StartTls);

                Console.WriteLine("SMTP connected");

                await smtpClient.AuthenticateAsync(
                    _emailSettings.SenderEmail,
                    _emailSettings.Password);

                Console.WriteLine("SMTP authenticated");

                await smtpClient.SendAsync(email);

                Console.WriteLine("Email sent successfully");

                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email error:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}

