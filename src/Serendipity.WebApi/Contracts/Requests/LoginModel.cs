using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;
public class LoginModel
{
    [Required(ErrorMessage = "E-mail is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}