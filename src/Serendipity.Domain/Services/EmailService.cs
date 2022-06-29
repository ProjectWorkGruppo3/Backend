using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Interfaces.Services;

namespace Serendipity.Domain.Services;

public class EmailService : IEmailService
{
    private readonly AmazonSimpleEmailServiceClient _emailService;
    private readonly string _emailSender;

    public EmailService(AmazonSimpleEmailServiceClient emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _emailSender = configuration["AWS:SES:SenderEmail"];
    }


    public Task<bool> SendEmail(
        List<string> destinations,
        string subject,
        string? htmlBody,
        string? textBody
    )
    {
        var sendMailRequest = CreateEmailRequest(destinations, subject, htmlBody, textBody);

        return _emailService
            .SendEmailAsync(sendMailRequest)
            .ContinueWith(res => res.Exception == null);

    }

    private SendEmailRequest CreateEmailRequest(
        List<string> destinations,
        string subject,
        string? htmlBody,
        string? textBody
    )
    {
        var sendRequest = new SendEmailRequest
        {
            Source = _emailSender,
            Destination = new Destination
            {
                ToAddresses = destinations
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = htmlBody
                    },
                    Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = textBody
                    }
                }
            },
        };

        return sendRequest;
    }
}