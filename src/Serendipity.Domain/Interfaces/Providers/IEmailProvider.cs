using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Providers;

public interface IEmailProvider
{
    public Task<IResult> SendAlarmEmail(
        IEnumerable<string> destinations,
        string title,
        string message,
        string deviceId,
        DateTimeOffset date
    );

    public Task<IResult> SendResetEmail(string destination, string recoverToken);
}