using TrackItAll.Application.Dtos;

namespace TrackItAll.Application.Interfaces;

/// <summary>
/// An interface for a service that works with the queue system using azure storage queues.
/// </summary>
public interface IQueueService
{
    /// <summary>
    /// Adds a user email to the sign up queue.
    /// </summary>
    /// <param name="email">A user email to add to the sign up queue.</param>
    Task AddUserEmailToSignUpQueueAsync(string email);

    /// <summary>
    /// Adds a user email with the report to be sent to their email in the queue
    /// </summary>
    /// <param name="email">A user email to add to the report queue</param>
    /// <param name="responseDto">The report to add to queue</param>
    Task AddReportToSendInEmailQueueAsync(string email, ReportServiceResponseDto responseDto);
}