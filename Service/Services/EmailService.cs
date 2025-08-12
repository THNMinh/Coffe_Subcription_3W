using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using MimeKit.Text;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
namespace Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SmtpServer configuration is not set.");
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? throw new InvalidOperationException("SmtpPort configuration is not set."));
            _smtpUser = configuration["EmailSettings:SmtpUser"] ?? throw new InvalidOperationException("SmtpUser configuration is not set.");
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new InvalidOperationException("SMTP_PASSWORD environment variable is not set."); // Retrieve the password securely;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage()
            {
                From = { new MailboxAddress(Consts.COMPANY_NAME, _smtpUser) }, // Your name or company
                To = { new MailboxAddress(string.Empty, email) },
                Subject = subject,
                Body = new TextPart(TextFormat.Html) { Text = message }
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
