using Microsoft.Extensions.Logging;
using TrackItAll.Application.Interfaces;
using TrackItAll.Infrastructure.Authentication;

namespace TrackItAll.Application.Services;

/// <summary>
/// A service for account-related operations.
/// </summary>
/// <param name="queueService">An instance of <see cref="IQueueService"/> to interact with the queue</param>
/// <param name="azureAdB2CHelper">An instance of <see cref="AzureAdB2CHelper"/> to interact with Azure AD B2C</param>
/// <param name="logger">An instance of <see cref="ILogger{TCategoryName}"/> to log messages</param>
public class AccountService(
    IQueueService queueService,
    AzureAdB2CHelper azureAdB2CHelper,
    ILogger<AccountService> logger) : IAccountService
{
    /// <inheritdoc/>
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