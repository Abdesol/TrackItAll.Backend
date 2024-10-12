using Microsoft.Extensions.Logging;
using TrackItAll.Application.Interfaces;
using TrackItAll.Infrastructure.Authentication;

namespace TrackItAll.Application.Services;

public class AccountService(
    IQueueService queueService,
    ILogger<AccountService> logger) : IAccountService
{
    public async Task AddUserEmailToSignUpQueueAsync(string oid, string email)
    {
        try
        {
            await queueService.AddUserEmailToSignUpQueueAsync(email);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while queuing user email");
        }
    }
}