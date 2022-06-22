using System.ComponentModel.DataAnnotations.Schema;

namespace Serendipity.Infrastructure.Models;

public class EmergencyContact
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public virtual User User { get; set; }
}