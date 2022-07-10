namespace Serendipity.WebApi.Contracts.Responses;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime? Birthday{ get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    
    public string? Job { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

    public IEnumerable<string> EmergencyContacts { get; set; }  = Enumerable.Empty<string>();
}