using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartShopping.Lib.Models;

namespace SmartShopping.Lib.Api;

public sealed class OpenFoodFactsApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new IngredientJsonConverter(), new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNameCaseInsensitive = true
    };

    public OpenFoodFactsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BaseProduct> GetProductInformation(string barcode, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"product/{barcode}");
        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BaseProduct>(Options, cancellationToken) ??
                   new BaseProduct();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}