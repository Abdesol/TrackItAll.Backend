using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Functions;

public class UserOnboardingFunctions
{
    private readonly IEmailService _emailService;

    public UserOnboardingFunctions(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    [FunctionName("SendOnboardingEmailFunction")]
    public async Task SendOnboardingEmail(
        [QueueTrigger("user-signups", Connection = "AzureWebJobsStorage")]
        string userEmail)
    {
        await _emailService.SendOnboardingEmailAsync(userEmail);
    }
}