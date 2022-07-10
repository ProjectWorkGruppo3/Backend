using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class PasswordResetConfirmationRequest
{
    [Required]
    public string? Token { get; set; }
    [Required]
    public string? NewPassword { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}