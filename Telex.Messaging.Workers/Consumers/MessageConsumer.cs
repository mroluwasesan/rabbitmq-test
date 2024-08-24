using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MassTransit;
using SharedModels;
using Telex.Messaging.Workers.Models;

namespace Telex.Messaging.Workers.Consumers;

public class NotificationConsumer(ILogger<NotificationConsumer> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IConsumer<Notification>
{
    private readonly ILogger<NotificationConsumer> logger = logger;
    private readonly IHttpClientFactory clientFactory = clientFactory;
    private readonly IConfiguration configuration = configuration;

    private readonly HttpClient client = clientFactory.CreateClient();

    public async Task Consume(ConsumeContext<Notification> context)
    {

        AuthResponse authDetails = await GetAuthorisationTokenAsync();

        client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("bearer", authDetails.Data!.AccessToken);

        StringContent jsonRequestContent = new(JsonSerializer.Serialize(context.Message), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://api.staging.telex.im/api/v1/webhooks/feed/backend-queue", jsonRequestContent);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            logger.LogError("Failed sending {notification} to the server", JsonSerializer.Serialize(context.Message));
            Console.WriteLine(response.Content);
            return;
        }

        logger.LogInformation("Sent {notification} with response code: {code}", context.Message, response.StatusCode);
    }

    private async Task<AuthResponse> GetAuthorisationTokenAsync()
    {
        var loginDetails = configuration.GetSection("login");
        string? email = loginDetails.GetValue<string>("email");
        string? password = loginDetails.GetValue<string>("password");

        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        string jsonRequest = JsonSerializer.Serialize(new { email, password });

        StringContent httpContent = new(jsonRequest, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://api.staging.telex.im/api/v1/auth/login", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error response when getting auth token");
        }

        AuthResponse authResponse = (await response.Content.ReadFromJsonAsync<AuthResponse>())!;

        return authResponse;
    }
}
