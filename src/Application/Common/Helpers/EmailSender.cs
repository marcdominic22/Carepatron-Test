using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public class EmailSender
    {
        public static string sendEmail(string mail,string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("marcdominic.terrobias@gmail.com");
                message.To.Add(mail);
                message.Subject = subject;
                message.IsBodyHtml = true; 
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; 
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("juliemarc45@gmail.com", "mhwa tyuw bmpv vknt");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return "success";
            }
            catch (Exception e) {
                return e.Message;
            }
        }
    }
}