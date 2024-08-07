using Snacks.Api.Dtos;
using Snacks.Logic;

namespace Snacks.Api.HostedServices;

public class SendOrderToWebhookUrlService : IHostedService
{

    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<SendOrderToWebhookUrlService> _logger;

    public SendOrderToWebhookUrlService(IHttpClientFactory httpClientFactory, ILogger<SendOrderToWebhookUrlService> logger)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task SendOrderToWebhookUrlAsync(OrderForExternalApiDto order, IRestaurantLogic logic)
    {
        var client = httpClientFactory.CreateClient();
        var restaurantId = order.Order.RestaurantId;
        var restaurant = await logic.GetRestaurantAsync(restaurantId);
        if(restaurant is null)
            throw new ArgumentNullException(nameof(restaurant));
        var apiUrl = restaurant.WebhookUrl;

        // try to send order to webhook url maximal 30 times until success
        const int maxAttempts = 30;
        var waitTime = 3000;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                _logger.LogInformation($"SendOrderToWebhookURLService: Attempt {attempt} to send order to Restaurant {restaurantId}: {apiUrl}");
                var response = await client.PostAsJsonAsync(apiUrl, order);
                response.EnsureSuccessStatusCode();
                break;
            }
            catch (HttpRequestException)
            {

                if (attempt < maxAttempts)
                {
                    await Task.Delay(waitTime);
                    waitTime += 3000;
                }
                else // max tries reached
                {
                    _logger.LogError($"SendOrderToWebhookURLService: Failed to send order to Restaurant {restaurantId}: {apiUrl} after {attempt} attempts");
                    throw; 
                }
            }
        }
    }
}
