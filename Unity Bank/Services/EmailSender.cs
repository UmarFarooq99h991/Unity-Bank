using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Unity_Bank.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Fake email sender, just log the email
            Console.WriteLine($"Email Sent to {email}: {subject} - {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
