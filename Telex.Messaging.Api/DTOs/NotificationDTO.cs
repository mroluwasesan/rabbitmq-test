using System.Text.Json.Serialization;
using SharedModels;

namespace Telex.Messaging.Api.DTOs;

using System.Text.Json.Serialization;

public record NotificationDTO(
    [property: JsonPropertyName("action_type")] string ActionType,
    [property: JsonPropertyName("event_name")] string EventName,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("username")] string UserName
)
{
    // Static conversion operator to convert from NotificationDTO to Notification
    public static explicit operator Notification(NotificationDTO notificationDTO)
    {
        return new Notification
        {
            ActionType = notificationDTO.ActionType,
            EventName = notificationDTO.EventName,
            Status = notificationDTO.Status,
            UserName = notificationDTO.UserName,
        };
    }
}


