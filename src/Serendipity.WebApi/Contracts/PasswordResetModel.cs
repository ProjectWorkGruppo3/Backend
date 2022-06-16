namespace Serendipity.WebApi.Contracts;

public class PasswordResetModel
{
    public string? Token { get; set; }
    public string? NewPassword { get; set; }
}

public class PasswordResetRequestModel
{
    public string? Password { get; set; }
}