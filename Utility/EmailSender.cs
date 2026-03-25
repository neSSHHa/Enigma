using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;
namespace Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Mail configuration
            var emailToSend = new MimeMessage();
            emailToSend.To.Add(MailboxAddress.Parse(email));

            // Configuring subject and body of mail.
            emailToSend.Subject = subject;
            emailToSend.From.Add(MailboxAddress.Parse("your email here"));
            emailToSend.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            // Connecting to mail and sending that mail.
            using var emailClient = new SmtpClient();
            // Making connection
            await emailClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            // Authentication
            await emailClient.AuthenticateAsync("your email here", "your token here");


            // Sending email
            await emailClient.SendAsync(emailToSend);

            // Disconnecting Clinet.
            await emailClient.DisconnectAsync(true);

        }
    }
}
