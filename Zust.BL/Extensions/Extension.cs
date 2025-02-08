namespace Zust.BL.Extensions;

public static class Extension
{
    public static string HideEmailInfo(this string email)
    {
        string[] spt = email.Split('@');

        if (spt[0].Length >= 6)
        {
            string localPart = spt[0];
            string modifiedLocalPart = localPart.Substring(0, 3) + new string('*', localPart.Length - 6) + localPart.Substring(localPart.Length - 3);
            spt[0] = modifiedLocalPart;
        }

        return String.Join('@', spt);
    }
}
