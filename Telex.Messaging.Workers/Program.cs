using MassTransit;
using Telex.Messaging.Workers.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();

var rabbit = builder.Configuration.GetSection("RabbitMq");
builder.Services.AddMassTransit((configurator) =>
{
    configurator.AddConsumer<NotificationConsumer>();
    configurator.UsingRabbitMq((context, config) =>
    {
        config.Host(rabbit.GetValue<string>("HostName"), h =>
        {
            h.Username(rabbit.GetValue<string>("UserName"));
            h.Password(rabbit.GetValue<string>("Password"));
            
        });
        config.ConfigureEndpoints(context);
    });
});


var host = builder.Build();
host.Run();
