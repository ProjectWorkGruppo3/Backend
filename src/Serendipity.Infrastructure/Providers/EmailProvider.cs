using System.Net;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Providers;

namespace Serendipity.Infrastructure.Providers;

public class EmailProvider : IEmailProvider
{
    private readonly AmazonSimpleEmailServiceClient _emailService;
    private readonly string _emailSender;

    public EmailProvider(AmazonSimpleEmailServiceClient emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _emailSender = configuration["AWS:SES:SenderEmail"];
    }


    public async Task<IResult> SendEmail(
        List<string> destinations,
        string subject,
        string? htmlBody,
        string? textBody
    )
    {
        try
        {
            var sendMailRequest = CreateEmailRequest(destinations, subject, htmlBody, textBody);

            var res = await _emailService.SendEmailAsync(sendMailRequest);

            if (res.HttpStatusCode != HttpStatusCode.OK)
            {
                return new ErrorResult("Something went wrong.");
            }

            return new SuccessResult();
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
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