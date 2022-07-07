namespace Serendipity.WebApi.Contracts.Responses;

public class PaginateAlarmResponse
{
    public int Total { get; set; }
    
    public IEnumerable<AlarmResponse> Data { get; set; }
    
    public int Start { get; set; }
    
    public int Limit { get; set; }
}