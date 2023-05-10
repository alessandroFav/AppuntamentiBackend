using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace provaProgetto.Controllers
{
    [Route("mail")]
    public class MailController:Controller
    {
        [Route("send")]
        public IActionResult SendEmail()
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("75dcceeb35360d", "f26671de2e9456"),
                EnableSsl = true
            };

            string body = System.IO.File.ReadAllText("Controllers/mailFormatting.html");

            MailMessage msg = new MailMessage("from@example.com", "to@example.com", "Hello world", body);
            msg.IsBodyHtml = true;
            client.Send(msg);
            return Ok("sent");
        }

        
    }
}

