namespace Carpool.Application.Common.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string userEmail, string emailSubject, string emailBody);
}
