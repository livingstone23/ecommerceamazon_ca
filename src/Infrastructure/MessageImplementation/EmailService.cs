using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ecommerce.Infrastructure.MessageImplementation;

    public class EmailService : IEmailServices
    {
        public EmailSettings _emailSettings {get;}

        public ILogger<EmailService> _logger {get;}

        public EmailService(IOptions<EmailSettings> emailSettings, 
                ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }
        
        public async Task<bool> SendEmail(EmailMessage email, string token)
        {
            try
            {
                var client = new SendGridClient(_emailSettings.Key);
                var from = new EmailAddress(_emailSettings.Email);
                var Subject = email.Subject;
                var to = new EmailAddress(email.To, email.To);
                
                var plainTextContent = email.Body;
                var htmlContent = $"{email.Body}{_emailSettings.BaseUrlClient}/password/reset/{token}";
                var ms = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(ms);
                return response.IsSuccessStatusCode;

            }
            catch(Exception ex)
            {
                _logger.LogError("El email no se pudo enviar",ex.Message );
                return false;
            }
        }

    
}
