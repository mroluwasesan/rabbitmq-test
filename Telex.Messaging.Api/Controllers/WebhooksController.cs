using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using Telex.Messaging.Api.DTOs;

namespace Telex.Messaging.Api.Controllers;

[ApiController]
[Route("/api/v1/webhooks")]
public class WebhooksController(IPublishEndpoint publish, ILogger<IPublishEndpoint> logger) : ControllerBase
{
    private readonly IPublishEndpoint publisher = publish;
    private readonly ILogger<IPublishEndpoint> logger = logger;

    [HttpPost("feed/backend-queue")]
    public ActionResult PublishMessageFromBody([FromBody] Notification notification)
    {
        Task task = new(async () =>
        {
            logger.LogInformation("Publishing created {notification} at {time}", notification, DateTimeOffset.UtcNow);
            await publisher.Publish(notification);
        });
        task.Start();
        return new ObjectResult(notification) { StatusCode = 201 };
    }

    [HttpPost("feed/{webhook_slug}")]
    public ActionResult PublishMessageFromRoute([FromRoute] Guid webhook_slug, [FromBody] NotificationDTO notificationDTO)
    {
        Notification notification = (Notification)notificationDTO with { ChannelId = webhook_slug };
        Task task = new(async () =>
        {
            logger.LogInformation("Publishing created {notification} at {time}", notification, DateTimeOffset.UtcNow);
            await publisher.Publish(notification);
        });
        task.Start();
        return new ObjectResult(notification) { StatusCode = 201 };

    }

    [HttpGet("feed/{webhook_slug}")]
    public ActionResult PublishMessageFromQuery([FromRoute] Guid webhook_slug, [FromQuery] FromQueryNotificationDTO queryNotificationDTO)
    {
        Notification notification = (Notification)queryNotificationDTO with {ChannelId = webhook_slug};
        Task task = new(async () =>
        {
            logger.LogInformation("Publishing created {notification} at {time}", notification, DateTimeOffset.UtcNow);
            await publisher.Publish(notification);
        });
        task.Start();
        return new ObjectResult(notification) { StatusCode = 201 };
    }
}
