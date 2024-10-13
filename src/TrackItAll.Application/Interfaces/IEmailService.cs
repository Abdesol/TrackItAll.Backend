namespace TrackItAll.Application.Interfaces;

/// <summary>
/// An interface for the email service.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an onboarding email to the user.
    /// </summary>
    /// <param name="email">email of the new user to send the email to</param>
    Task SendOnboardingEmailAsync(string email);
}