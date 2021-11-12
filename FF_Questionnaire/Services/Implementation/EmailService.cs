using FF_Questionnaire.Services.Interface;
using System.Net.Mail;

namespace FF_Questionnaire.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void SendEmail(string subject, string email)
        {
            string targetEmail = _configuration["EmailAddress"];

            MailMessage mailMessage = new("noreply@quizff.com", targetEmail, subject, email);

            SmtpClient smtpClient = new("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("fwknfkwenkcnjwebcwencwb@gmail.com", "qweQWE123!@#")
            };

            smtpClient.Send(mailMessage);
        }
    }
}
