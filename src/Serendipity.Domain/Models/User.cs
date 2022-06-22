using Microsoft.AspNetCore.Identity;

namespace Serendipity.Domain.Models;

public class User : IdentityUser
{
    public string? ProPicUrl { get; set; }
    public decimal Weight { get; set; }
    public DateTime DayOfBirth { get; set; }
    public decimal Height { get; set; }
}