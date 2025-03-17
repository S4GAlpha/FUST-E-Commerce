using ECommerce.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace ECommerce.Components.Account
{
    // Remove the "else if (EmailSender is IdentityNoOpEmailSender)" block from RegisterConfirmation.razor after updating with a real implementation.
    internal sealed class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public IdentityNoOpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            // emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["name"], _configuration["email"]));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Confirm your email";

            message.Body = new TextPart("html")
            {
                Text = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>."
            };


            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_configuration["email"], _configuration["password"]);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            //  emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["name"], _configuration["email"]));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Reset your password";

            message.Body = new TextPart("html")
            {
                Text = $"Please reset your password by <a href='{resetLink}'>clicking here</a>."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_configuration["email"], _configuration["password"]);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            // emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["name"], _configuration["email"]));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Reset your password";

            message.Body = new TextPart("html")
            {
                Text = $"Please reset your password using the following code: {resetCode}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_configuration["email"], _configuration["password"]);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
