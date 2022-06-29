﻿using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Providers;

public interface IEmailProvider
{
    public Task<IResult> SendEmail(
        List<string> destinations,
        string subject,
        string? htmlBody,
        string? textBody
    );
}