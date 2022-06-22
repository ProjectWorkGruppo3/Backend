using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts;

public class AdminRegisterModel
{
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}