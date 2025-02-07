namespace Zust.BL.ExternalServices.Interfaces;

public interface IUserClaimService
{
    string GetUserName();
    string GetRole();
    string GetEmail();
    string GetId();
    string GetFirstName();
    string GetLastName();
    string GetFullName();
}
