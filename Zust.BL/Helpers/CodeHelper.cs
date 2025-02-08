namespace Zust.BL.Helpers;

public class CodeHelper
{
    public static string GenerateCode()
    {
        Random random = new Random();
        int sixDigitCode = random.Next(100000, 1000000);
        return Convert.ToString(sixDigitCode);
    }
}
