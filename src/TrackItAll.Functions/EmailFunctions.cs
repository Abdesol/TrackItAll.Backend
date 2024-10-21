using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Functions;

/// <summary>
/// Functions class for email sending azure function units
/// </summary>
/// <param name="emailService">An instance of <see cref="IEmailService"/> to send emails.</param>
public class EmailFunctions(IEmailService emailService)
{
    /// <summary>
    /// Sends an onboarding email to a user when a message is received from the "user-signups" queue.
    /// </summary>
    /// <param name="userEmail">The email address of the user to send the onboarding email to, retrieved from the queue message.</param>
    /// <remarks>
    /// This function is triggered by a message in the "user-signups" Azure Storage Queue. 
    /// The message content is expected to be the user's email address, and the function will use this to send an onboarding email.
    /// </remarks>
    [FunctionName("SendOnboardingEmailFunction")]
    public async Task SendOnboardingEmail(
        [QueueTrigger("user-signups", Connection = "AzureWebJobsStorage")]
        string userEmail)
    {
        await emailService.SendOnboardingEmailAsync(userEmail);
    }
    
    /// <summary>
    /// Sends an onboarding email to a user when a message is received from the "user-signups" queue.
    /// </summary>
    /// <param name="report">The report to send to the user's email, retrieved from the queue message.</param>
    /// <remarks>
    /// This function is triggered by a message in the "report-to-send-in-email" Azure Storage Queue. 
    /// The message content is expected to be the report object and email serialized together.
    /// </remarks>
    [FunctionName("SendReportToEmail")]
    public async Task SendReportToEmail(
        [QueueTrigger("report-to-send-in-email", Connection = "AzureWebJobsStorage")]
        string report)
    {
        await emailService.SendReportAsync(report);
    }
}