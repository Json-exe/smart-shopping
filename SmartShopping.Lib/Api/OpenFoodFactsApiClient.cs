namespace SmartShopping.Lib.Api;

public sealed class OpenFoodFactsApiClient
{
    private readonly HttpClient _httpClient;

    public OpenFoodFactsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task GetProductInformation(string barcode, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/product/{barcode}");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}