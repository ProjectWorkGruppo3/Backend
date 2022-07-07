using System.Net;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Providers;

namespace Serendipity.Infrastructure.Providers;

public class EmailProvider : IEmailProvider
{
    private readonly ISendGridClient _emailService;
    private readonly string _fromEmail;
    private readonly string _emailTemplateId;
    private readonly string _callbackUrl;

    public EmailProvider(ISendGridClient emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _fromEmail = configuration["Email:From"];
        _emailTemplateId = configuration["Email:TemplateId"];
        _callbackUrl = configuration["Email:CallbackUrl"];
    }


    public async Task<IResult> SendAlarmEmail(
        IEnumerable<string> destinations, 
        string title,
        string message,
        string deviceId,
        DateTimeOffset date
    )
    {
        try
        {

            var email = MailHelper.CreateSingleEmailToMultipleRecipients(
                new EmailAddress(_fromEmail),
                destinations.Select(el => new EmailAddress(el)).ToList(),
                "",
                "",
                ""
            );
            
            
            email.SetTemplateId(_emailTemplateId);
            email.SetTemplateData(new
            {
                title=title,
                message=message,
                date=$"{date:d} at {date:t}",
                link=$"{_callbackUrl}/{deviceId}/alarms"
            });


            var result = await _emailService.SendEmailAsync(email);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Message not sent");
            }
            
            
            

            return new SuccessResult();
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    
}