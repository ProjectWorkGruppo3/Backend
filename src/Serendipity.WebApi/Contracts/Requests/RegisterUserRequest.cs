using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class RegisterUserRequest
{
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    
    
    [Required(ErrorMessage = "DayOfBirth is required")]
    public DateTime DayOfBirth { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    public decimal Weight { get; set; }

    [Required(ErrorMessage = "Height is required")]
    public decimal Height { get; set; }
    public string? Job { get; set; }
}