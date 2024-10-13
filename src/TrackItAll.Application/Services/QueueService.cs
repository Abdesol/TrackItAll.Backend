using System.Security;
using System.Text;
using Azure.Storage.Queues;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Application.Services;

/// <summary>
/// Service for adding messages to the Azure Queue Storage.
/// </summary>
/// <param name="connectionString">The connection string to the Azure Queue Storage Account.</param>
public class QueueService(string connectionString) : IQueueService
{
    /// <inheritdoc />
    public async Task AddUserEmailToSignUpQueueAsync(string email)
    {
        var queueClient = new QueueClient(connectionString, "user-signups");
        await queueClient.CreateIfNotExistsAsync();

        if (!await queueClient.ExistsAsync()) return;
        var message = Convert.ToBase64String(Encoding.UTF8.GetBytes(email));
        await queueClient.SendMessageAsync(message);
    }
}