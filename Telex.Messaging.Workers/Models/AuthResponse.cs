using System.Text.Json.Serialization;

namespace Telex.Messaging.Workers.Models;

public record AuthResponse
{
    [JsonPropertyName("data")]
    public Data? Data { get; init; }
}

public record Data
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }
}