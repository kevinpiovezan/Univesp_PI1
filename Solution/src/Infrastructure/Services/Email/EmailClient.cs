using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services.Email
{
    public class EmailClient : EmailService
    {
        private readonly string host = AppSettings.Configuration.GetConnectionString("SmtpHost");
        private readonly string emailSender = AppSettings.Configuration.GetConnectionString("SmtpEmailSender");
        private readonly string password = AppSettings.Configuration.GetConnectionString("SmtpPassword");
        private readonly int emailPort = int.Parse(AppSettings.Configuration.GetConnectionString("SmtpPort"));

        public async Task<bool> EnviarEmail(EmailDTO email)
        {
            using (MailMessage mailMessage = new MailMessage(emailSender,
                                                             email.ToEmail,
                                                             email.Subject,
                                                             email.Body))
            {
                mailMessage.IsBodyHtml = true;

                using SmtpClient emailClient = new SmtpClient(host)
                {
                    Port = emailPort,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailSender, password),
                    EnableSsl = true
                };

                AdicionarImagemAoEmail(mailMessage, email.Body);

                await emailClient.SendMailAsync(mailMessage);
            };

            return true;
        }
    }
}
