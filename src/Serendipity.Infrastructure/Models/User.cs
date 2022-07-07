using Microsoft.AspNetCore.Identity;

namespace Serendipity.Infrastructure.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? ProPicUrl { get; set; }
    public virtual ICollection<Device> Devices { get; set; } = null!;
    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = null!;
    public virtual PersonalInfo? PersonalInfo { get; set; }
}