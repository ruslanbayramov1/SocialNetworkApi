namespace Zust.BL.Options;

public class JwtOption
{
    public const string Position = "JwtOptions";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
}
