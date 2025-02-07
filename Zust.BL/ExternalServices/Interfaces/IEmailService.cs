namespace Zust.BL.ExternalServices.Interfaces;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string username, string code, string userEmail); 
}
