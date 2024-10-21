using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackItAll.Application.Dtos;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Application.Services;

/// <summary>
/// A service for sending emails.
/// </summary>
/// <param name="configuration">An instance of <see cref="IConfiguration"/> to access the configuration settings</param>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task SendReportAsync(string report)
    {
        var reportModel = JsonConvert.DeserializeObject<Dictionary<string, object?>>(report);

        var emailObject = reportModel?["Email"];
        var reportObject = reportModel?["Report"];
        if (emailObject is null || reportObject is null) return;

        string? email;
        ReportServiceResponseDto? reportDto;
        try
        {
            email = emailObject as string;
            reportDto = reportObject as ReportServiceResponseDto;
        }
        catch (Exception)
        {
            // ignore
            return;
        }

        var emailBody = $"""

                         Hello,

                         Here is your expense report for the period from {reportDto?.ReportStartDate:MMMM dd, yyyy} to {reportDto?.ReportEndDate:MMMM dd, yyyy}:

                         -----------------------------------------------------
                         Total Expenses:           {reportDto?.TotalExpensesAmount:C}
                         Most Expensive Expense:   {reportDto?.HighestExpense?.Description ?? "N/A"} - {reportDto?.HighestExpense?.Amount:C}
                         Least Expensive Expense:  {reportDto?.LowestExpense?.Description ?? "N/A"} - {reportDto?.LowestExpense?.Amount:C}
                         Top Category Spent On:    {reportDto?.TopCategorySpentOn?.Name ?? "N/A"}
                         -----------------------------------------------------

                         Thank you for using TrackItAll to manage your expenses!

                         Best regards,
                         The TrackItAll Team
                             
                         """;
        var mailMessage = new MailMessage
        {
            From = new MailAddress(configuration["Smtp:Username"]!),
            Subject = "Expense Report Data",
            Body = emailBody,
            IsBodyHtml = true,
            To = { email! },
        };

        await _smtpClient.SendMailAsync(mailMessage);
    }
}