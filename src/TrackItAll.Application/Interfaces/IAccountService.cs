namespace TrackItAll.Application.Interfaces;

/// <summary>
/// An interface for the account service.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Adds a user to the sign up queue.
    /// </summary>
    /// <param name="oid">object id of the user</param>
    /// <param name="email">email of the user</param>
    Task AddUserEmailToSignUpQueueAsync(string oid, string email);
}