namespace Serendipity.Domain.Interfaces.Services;

public interface IEmailService
{
    public Task<bool> SendEmail(
        List<string> destinations,
        string subject,
        string? htmlBody,
        string? textBody
    );
}