using SmartShopping.Lib.Database.Models;

namespace SmartShopping.Lib.Services;

public sealed class NotificationService
{
    private readonly HttpClient _notificationClient;

    public NotificationService()
    {
        _notificationClient = new HttpClient();
        _notificationClient.BaseAddress = new Uri("https://alertzy.app/");
    }

    public async Task SendProductExpirationNotification(Product product)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "send");
        message.Content = CreateProductNotificationContent(product);
        var response = await _notificationClient.SendAsync(message);
        response.EnsureSuccessStatusCode();
    }

    private static FormUrlEncodedContent CreateProductNotificationContent(Product product)
    {
        var content = new Dictionary<string, string>
        {
            { "accountKey", "" },
            { "title", product.Name + " is expiring soon" }
        };

        var expirationDifference = product.ExpirationDate - DateTime.Now;
        var priority = expirationDifference.Days switch
        {
            <= 1 => Priority.Critical,
            <= 3 => Priority.High,
            _ => Priority.Normal
        };

        content.Add("message", product.Name +
                               $" is expiring at {product.ExpirationDate:d} (in {expirationDifference.Days} days). Please eat it to not " +
                               "throw away any food. If you already ate it, you can remove it from the tracking.");
        content.Add("priority", ((int)priority).ToString());

        return new FormUrlEncodedContent(content);
    }
}

file enum Priority
{
    Normal = 0,
    High = 1,
    Critical = 2
}