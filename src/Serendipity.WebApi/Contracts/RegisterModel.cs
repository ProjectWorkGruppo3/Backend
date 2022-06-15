using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts;

public class RegisterModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
    public string? ProPicUrl { get; set; }
    
    [Required(ErrorMessage = "Weight is required")]
    public decimal Weight { get; set; }
    
    [Required(ErrorMessage = "DayOfBirth is required")]
    public DateTime DayOfBirth { get; set; }
    
    [Required(ErrorMessage = "Height is required")]
    public decimal Height { get; set; }
}