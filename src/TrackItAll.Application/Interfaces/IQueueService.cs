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
}