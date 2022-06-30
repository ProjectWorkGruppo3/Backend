using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class UpdateUserRequest
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
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