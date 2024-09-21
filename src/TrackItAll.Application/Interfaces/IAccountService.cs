namespace TrackItAll.Application.Interfaces;

public interface IAccountService
{
    Task AddUserEmailToSignUpQueueAsync(string oid, string email);
}