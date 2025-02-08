using Zust.BL.Enums;

namespace Zust.BL.ExternalServices.Interfaces;

public interface IEmailService
{
    Task SendCodeToEmailAsync(string username, string code, string userEmail, EmailTypes emailType); 
}
