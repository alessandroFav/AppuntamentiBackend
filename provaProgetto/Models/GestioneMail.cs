using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Razor.Templating.Core;
using System.Dynamic;

namespace provaProgetto.Models
{
    public class GestioneMail
    {
        private async Task<string> ConvertView(string viewName, object Tmodel, string baseURL)
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.model = Tmodel;
            mymodel.baseURL = baseURL;
            var html = await RazorTemplateEngine.RenderAsync(viewName, mymodel);
            return html;
        }

        public async Task<bool> SendMailVerificationAsync(int userId, string baseURL)
        {
            try
            {
                var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("0c401e50958023", "fbefeeaf03e1f6"),
                    EnableSsl = true
                };

                string body = await ConvertView("Htmls/mailVerification.cshtml", userId, baseURL);

                MailMessage msg = new MailMessage("from@example.com", "to@example.com", "Mail Verification", body);
                msg.IsBodyHtml = true;
                client.Send(msg);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
