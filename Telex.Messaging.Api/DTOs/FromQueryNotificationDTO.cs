using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace Telex.Messaging.Api.DTOs;

public record FromQueryNotificationDTO(
    // [property: FromQuery(Name = "action_type")]
     string action_type,
    // [property: FromQuery(Name = "event_name")] 
    string event_name,
    [property: FromQuery(Name = "status")] string Status,
    [property: FromQuery(Name = "username")] string UserName
)
{
    public static explicit operator Notification(FromQueryNotificationDTO notificationDTO)
    {
        return new Notification()
        {
            ActionType = notificationDTO.action_type,
            EventName = notificationDTO.event_name,
            Status = notificationDTO.Status,
            UserName = notificationDTO.UserName,
        };
    }
}
