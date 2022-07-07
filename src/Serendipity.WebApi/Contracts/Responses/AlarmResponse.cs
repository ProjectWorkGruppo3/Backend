namespace Serendipity.WebApi.Contracts.Responses;

public class AlarmResponse
{
    public string Type { get; set; }
    public DateTimeOffset Date { get; set; }
}