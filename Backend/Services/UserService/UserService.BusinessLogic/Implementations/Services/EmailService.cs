using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using UserService.BusinessLogic.Options;
using UserService.BusinessLogic.Specifications.Services;

namespace UserService.BusinessLogic.Implementations.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions= emailOptions.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName,_emailOptions.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            cancellationToken.ThrowIfCancellationRequested();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, true);
                await client.AuthenticateAsync(_emailOptions.SenderEmail, _emailOptions.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
