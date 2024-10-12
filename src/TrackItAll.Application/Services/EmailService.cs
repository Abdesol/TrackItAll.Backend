using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly SmtpClient _smtpClient = new(configuration["Smtp:Host"])
    {
        Port = int.Parse(configuration["Smtp:Port"]!),
        Credentials = new System.Net.NetworkCredential(
            configuration["Smtp:Username"], 
            configuration["Smtp:Password"]),
        EnableSsl = true,
    };
    
    public async Task SendOnboardingEmailAsync(string email)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(configuration["Smtp:Username"]!),
            Subject = "Welcome to TrackItAll!",
            Body = "<h1>Thank you for signing up!</h1>",
            IsBodyHtml = true,
            To = { email },
        };

        await _smtpClient.SendMailAsync(mailMessage);
    }
}