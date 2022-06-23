using System.ComponentModel.DataAnnotations;

namespace Serendipity.WebApi.Contracts.Requests;

public class RegisterDeviceRequest
{
    [Required]
    public Guid? Id { get; set; }
    [Required] 
    public string? Name { get; set; }
}