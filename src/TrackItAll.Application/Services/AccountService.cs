using Microsoft.Extensions.Logging;
using TrackItAll.Application.Interfaces;
using TrackItAll.Infrastructure.Authentication;

namespace TrackItAll.Application.Services;

public class AccountService(
    IQueueService queueService,
    AzureAdB2CHelper azureAdB2CHelper,
    ILogger<AccountService> logger) : IAccountService
{
    public async Task AddUserEmailToSignUpQueueAsync(string oid, string email)
    {
        try
        {
            if (!await azureAdB2CHelper.IsUserOnBoarded(oid))
            {
                await queueService.AddUserEmailToSignUpQueueAsync(email);
                await azureAdB2CHelper.UpdateUserOnBoardingStatusAsync(oid);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while queuing user email");
        }
    }
}