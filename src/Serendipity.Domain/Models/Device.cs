using System;

namespace Serendipity.Domain.Models;

public class Device
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
}