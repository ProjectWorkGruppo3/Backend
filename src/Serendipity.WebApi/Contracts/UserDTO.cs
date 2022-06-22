namespace Serendipity.WebApi.Contracts;

public class UserDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime? BirthDay{ get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}