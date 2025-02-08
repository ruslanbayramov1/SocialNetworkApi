using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Zust.BL.Constants;
using Zust.BL.Enums;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Options;

namespace Zust.BL.ExternalServices.Implements;

public class EmailService : IEmailService
{
    private readonly SmtpClient _client;
    private readonly MailAddress _from;
    private readonly HttpContext _httpContext;
    public EmailService(IHttpContextAccessor http, IOptions<SmtpOption> opt)
    {
        SmtpOption _opt = opt.Value;
        _httpContext = http.HttpContext!;

        _client = new SmtpClient(_opt.Host, _opt.Port);
        _client.EnableSsl = true;
        _client.Credentials = new NetworkCredential(_opt.Sender, _opt.Password);

        _from = new MailAddress(_opt.Sender, "Zust");
    }
    public Task SendCodeToEmailAsync(string username, string code, string userEmail, EmailTypes emailType)
    {
        MailAddress to = new MailAddress(userEmail);

        MailMessage message = new MailMessage(_from, to);

        message.IsBodyHtml = true;

        if (emailType == EmailTypes.Confirmation)
        { 
            message.Body = EmailTemplate.ConfirmTemplate.Replace("__$appName", "Zust").Replace("__$code", code).Replace("__$userName", username);
            message.Subject = "Zust Email Confirmation";
        }

        else if (emailType == EmailTypes.ForgotPassword)
        { 
            message.Body = EmailTemplate.ForgotPasswordTemplate.Replace("__$appName", "Zust").Replace("__$code", code).Replace("__$userName", username);
            message.Subject = "Zust Forgot Password";
        }

        else if (emailType == EmailTypes.NewPassword)
        { 
            message.Body = EmailTemplate.NewPasswordTemplate.Replace("__$appName", "Zust").Replace("__$code", code).Replace("__$userName", username);
            message.Subject = "Zust New Password";
        }


        _client.Send(message);

        return Task.CompletedTask;
    }
}
