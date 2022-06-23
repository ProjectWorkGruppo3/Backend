using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class PasswordResetRequest
{
    [Required]
    public string? Email { get; set; }
}