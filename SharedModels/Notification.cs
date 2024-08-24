using System.Text.Json.Serialization;

namespace SharedModels;
public record Notification
{
    [JsonPropertyName("channel_id")]
    public Guid ChannelId { get; init; }

    [JsonPropertyName("action_type")]
    public string? ActionType { get; init; }

    [JsonPropertyName("event_name")]
    public string? EventName { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("username")]
    public string? UserName { get; init; }
}