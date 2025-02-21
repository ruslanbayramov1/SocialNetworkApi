namespace Zust.DAL.Options;

public class MongoOption
{
    public const string Position = "MongoOptions";
    public const string DatabaseName = "Users";
    public string Connection { get; set; }
    public string Password { get; set; }
}
