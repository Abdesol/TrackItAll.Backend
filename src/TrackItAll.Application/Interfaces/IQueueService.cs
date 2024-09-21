namespace TrackItAll.Application.Interfaces;

public interface IQueueService
{
    Task AddUserEmailToSignUpQueueAsync(string email);
}