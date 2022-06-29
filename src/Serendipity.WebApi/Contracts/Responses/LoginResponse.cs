namespace Serendipity.WebApi.Contracts.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserResponse User { get; set; }
}