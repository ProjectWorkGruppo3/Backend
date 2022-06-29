using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class UpdateAdminRequest
{
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; set; }
    
    // public string? ProfilePic { get; set; }
}