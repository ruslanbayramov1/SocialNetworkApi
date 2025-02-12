namespace Zust.BL.Options;

public class MongoOption
{
    public const string Position = "MongoOption";
    public const string DatabaseName = "Users";
    public string Connection { get; set; }
    public string Password { get; set; }
}
