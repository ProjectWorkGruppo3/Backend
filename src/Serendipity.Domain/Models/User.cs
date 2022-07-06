namespace Serendipity.Domain.Models;

public class User
{
    public string? ProPicUrl { get; set; }
    public decimal? Weight { get; set; }
    public DateTime? DayOfBirth { get; set; }
    public decimal? Height { get; set; }
    public string? Job { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public Guid Id { get; set; }
}