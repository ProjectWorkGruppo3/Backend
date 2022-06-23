namespace Serendipity.Infrastructure.Models;

public class PersonalInfo
{
    public decimal Weight { get; set; }
    public DateTime BirthDay { get; set; }
    public decimal Height { get; set; }
    public string? Job { get; set; }
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}