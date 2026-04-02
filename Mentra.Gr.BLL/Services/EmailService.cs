using Mentra.Gr.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var mail = new MailMessage();
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            mail.From = new MailAddress("ahmedsabry010oggy@gmail.com");

            var smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential("ahmedsabry010oggy@gmail.com", "fxqmxaamwsvixvmd");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
        }

    }
}
