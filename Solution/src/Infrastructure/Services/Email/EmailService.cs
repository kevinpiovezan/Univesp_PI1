using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services.Email
{
    public class EmailService //: IEmailService
    {
        private readonly string AppBaseUrl = AppSettings.Configuration.GetSection("AppInformation")["AppBaseURL"];

        public async Task<bool> PrepararEnvioEmail(EmailDTO emailData)
        {
            EmailClient emailClient = new EmailClient();
#if DEBUG
            emailData.ToEmails = new List<string> { "kevin.piovezan@univesp.com" };
#endif
            emailData.Subject = "Nova auditoria adicionada";
            emailData.Body = LoadEmailBody(emailData);
            foreach (var e in emailData.ToEmails)
            {
                emailData.ToEmail = e;
                await emailClient.EnviarEmail(emailData);
            }
            return true;
        }

        public void AdicionarImagemAoEmail(MailMessage mailMessage, string body)
        {
            const string tipoArquivo = "image/png";
            string _rootFolder = Path.Combine("wwwroot", "img", "logos");
            string caminhoImagemLogo = Path.Combine(_rootFolder, "logo_white-univesp.png");

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
            LinkedResource emailLogoImage = new LinkedResource(caminhoImagemLogo, tipoArquivo)
            {
                ContentId = "univesp_logo"
            };

            htmlView.LinkedResources.Add(emailLogoImage);
            mailMessage.AlternateViews.Add(htmlView);
        }

        public string LoadEmailBody(EmailDTO emailData)
        {
            string _rootFolder = Path.Combine("wwwroot", "template", "email");
            string arquivoHtml = Path.Combine(_rootFolder, "nova_auditoria.html");

            string body = string.Empty;

            using (StreamReader arquivoEmail = new StreamReader(arquivoHtml))
            {
                body = arquivoEmail.ReadToEnd();
            }

            return body.Replace("{Link_Ajuste}", AppBaseUrl + "/auditoria/" + emailData.Id.ToString());
        }
    }
}

