using FinalProject.Application.Common.Interfaces;
using FinalProject.Application.Common.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Infrastructure.Services
{
    public class EmailManager : IEmailService
    {

        public void SendEmailStatus(SendEmailStatusDto sendEmailStatusDto)
        {
            var htmlContent = $"<h4>{sendEmailStatusDto.Message} </h4>";
            var subject = "The latest on the crawling process";

            Send(new SendEmailDto(sendEmailStatusDto.Email, htmlContent, subject));
        }

        private void Send(SendEmailDto sendEmailDto)
        {
            MailMessage message = new MailMessage();

            sendEmailDto.EmailAddresses.ForEach(emailAddress => message.To.Add(emailAddress));

            message.From = new MailAddress("noreply@entegraturk.com");

            message.Subject = sendEmailDto.Subject;

            message.IsBodyHtml = true;

            message.Body = sendEmailDto.Content;

            SmtpClient client = new SmtpClient();

            client.Port = 587;

            client.Host = "mail.entegraturk.com";

            client.EnableSsl = false;

            client.UseDefaultCredentials = false;

            client.Credentials = new NetworkCredential("noreply@entegraturk.com", "xzx2xg4Jttrbzm5nIJ2kj1pE4l");

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Send(message);


        }
    }
}
