using System.Text.Json.Serialization;

namespace SmartShopping.Lib.Models;

public sealed record BaseProduct
{
    public string Code { get; init; }
    public int Status { get; init; }
    [JsonPropertyName("status_verbose")] public string StatusVerbose { get; init; }
    public Product Product { get; init; }
}