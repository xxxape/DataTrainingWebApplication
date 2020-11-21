using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Data_Training.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "#SenderAPIKEY";

        public void Send(String toEmail, String subject, String contents, String filePath, String fileName)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("justxxxape@gmail.com", "Data Training");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            if(filePath != null && filePath != "")
            {
                var bytes = File.ReadAllBytes(filePath);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment(fileName, file);
            }
            var response = client.SendEmailAsync(msg);
        }
    }
}
