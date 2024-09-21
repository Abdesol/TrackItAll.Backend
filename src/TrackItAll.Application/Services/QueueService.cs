using System.Security;
using System.Text;
using Azure.Storage.Queues;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Application.Services;

public class QueueService(string connectionString) : IQueueService
{
    public async Task AddUserEmailToSignUpQueueAsync(string email)
    {
        var queueClient = new QueueClient(connectionString, "user-signups");
        await queueClient.CreateIfNotExistsAsync();

        if (!await queueClient.ExistsAsync()) return;
        var message = Convert.ToBase64String(Encoding.UTF8.GetBytes(email));
        await queueClient.SendMessageAsync(message);
    }
}