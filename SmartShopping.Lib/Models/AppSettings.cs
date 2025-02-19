namespace SmartShopping.Lib.Models;

public sealed record AppSettings
{
    public const string KeyIdentifier = "AppSettings";
    public string NotificationKey { get; set; } = string.Empty;
}