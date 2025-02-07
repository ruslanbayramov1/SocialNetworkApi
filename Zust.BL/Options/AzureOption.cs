namespace Zust.BL.Options;

public class AzureOption
{
    public const string Position = "AzureOptions";
    public string Connection { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string AccountKey { get; set; } = null!;
    public string ContainerName { get; set; } = null!;
}
