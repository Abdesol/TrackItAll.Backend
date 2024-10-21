using System.Security;
using System.Text;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using TrackItAll.Application.Dtos;
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

    public async Task AddReportToSendInEmailQueueAsync(string email, ReportServiceResponseDto responseDto)
    {
        var queueClient = new QueueClient(connectionString, "report-to-send-in-email");
        await queueClient.CreateIfNotExistsAsync();

        if (!await queueClient.ExistsAsync()) return;
        var messageObject = new
        {
            Email = email,
            Report = responseDto
        };
        var messageJson = JsonConvert.SerializeObject(messageObject);
        var message = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageJson));
        await queueClient.SendMessageAsync(message);
    }
}