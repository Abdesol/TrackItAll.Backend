namespace TrackItAll.Application.Interfaces;

public interface IEmailService
{
    Task SendOnboardingEmailAsync(string email);
}