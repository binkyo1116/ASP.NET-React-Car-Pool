namespace Carpool.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;

        }
        public async Task SendEmailAsync(string userEmail, string emailSubject, string emailBody)
        {
            var client = new SendGridClient(_config["Sendgrid:Key"]);
            var message = new SendGridMessage
            {
                From = new EmailAddress("remycedric@outlook.com", _config["SendGrid:User"]),
                Subject = emailSubject,
                PlainTextContent = emailBody,
                HtmlContent = emailBody
            };
            message.AddTo(new EmailAddress(userEmail, "Support Team"));
            await client.SendEmailAsync(message).ConfigureAwait(false);
        }
    }
}