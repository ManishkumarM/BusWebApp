
using System.Net.Mail;
using System.Net;
using System.Text;

namespace BusWebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<string> SendEmail(string toEmail, string subject)
        {
            try
            {
                var host = _configuration["EmailSettings:Host"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("busplanner@gmail.com");
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendFormat("<h1>User Registered</h1>");
                mailBody.AppendFormat("<br />");
                mailBody.AppendFormat("<p>Thank you For Registering account</p>");
                mailMessage.Body = mailBody.ToString();

                client.Send(mailMessage);
                //client.Send("bus123@gmail.com", "manishmanu34196@gmail.com", "Hello world", "testbody");
                return "Email sent successfully!";
            }
            catch (SmtpException ex)
            {
                return $"SMTP Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }


        //public async Task<string> SendEmail(string toEmail, string subject)
        //{
        //    try
        //    {
        //        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        //        var port = int.Parse(_configuration["EmailSettings:Port"]);
        //        var username = _configuration["EmailSettings:Username"];
        //        var password = _configuration["EmailSettings:Password"];
        //        // Set up SMTP client
        //        SmtpClient client = new SmtpClient("smtp.ethereal.email", 587);
        //        client.EnableSsl = true;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(username, password);

            //        // Create email message
            //        MailMessage mailMessage = new MailMessage();
            //        mailMessage.From = new MailAddress(username);
            //        mailMessage.To.Add(toEmail);
            //        mailMessage.Subject = subject;
            //        mailMessage.IsBodyHtml = true;
            //        StringBuilder mailBody = new StringBuilder();
            //        mailBody.AppendFormat("<h1>User Registered</h1>");
            //        mailBody.AppendFormat("<br />");
            //        mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            //        mailMessage.Body = mailBody.ToString();


            //        // Send email
            //        client.Send(mailMessage);
            //        return "Email sent successfully!";
            //    }
            //    catch (SmtpException ex)
            //    {
            //        return $"SMTP Error: {ex.Message}";
            //    }
            //    catch (Exception ex)
            //    {
            //        return $"An error occurred: {ex.Message}";
            //    }

            //}
    }
}
