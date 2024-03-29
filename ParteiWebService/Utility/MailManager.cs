﻿using System;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace ParteiWebService.Utility
{
    public class MailManager
    {
        public MailManager(){}

        public async Task SendEmail(string subject, string content, string destinationAddress, string destinationName, MemoryStream attachment, string fileName)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(destinationAddress, destinationName));
                message.From = new MailAddress("cloudbob46@gmail.com", "CloudBobs");
                message.Subject = subject;
                message.Body = content;
                message.IsBodyHtml = true;

                var contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                var reportAttachment = new Attachment(attachment, contentType);
                reportAttachment.Name = fileName;

                message.Attachments.Add(reportAttachment);

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("cloudbob46@gmail.com", "Cl0udb0b$");
                    client.EnableSsl = true;

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        public async Task SendEmail(string subject, string content, string destinationAddress, string destinationName)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(destinationAddress, destinationName));
                message.From = new MailAddress("cloudbob46@gmail.com", "CloudBobs");
                message.Subject = subject;
                message.Body = content;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("cloudbob46@gmail.com", "Cl0udb0b$");
                    client.EnableSsl = true;

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }


}
