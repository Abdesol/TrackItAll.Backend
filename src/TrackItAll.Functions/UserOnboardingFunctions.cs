using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Functions;

/// <summary>
/// Functions for user onboarding.
/// </summary>
/// <param name="emailService">An instance of <see cref="IEmailService"/> to send emails.</param>
public class UserOnboardingFunctions(IEmailService emailService)
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
}