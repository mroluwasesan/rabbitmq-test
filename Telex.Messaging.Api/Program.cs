using System.Text.Json;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen();

var rabbitConfig = builder.Configuration.GetSection("RabbitMq");

builder.Services.AddMassTransit((configurator) =>
{
    configurator.UsingRabbitMq((context, config) =>
    {
        config.Host(rabbitConfig.GetValue<string>("HostName"), h =>
        {
            string? rabbitUserName = rabbitConfig.GetValue<string>("UserName");
            string? rabbitPassword = rabbitConfig.GetValue<string>("Password");
            ArgumentException.ThrowIfNullOrWhiteSpace(rabbitUserName);
            ArgumentException.ThrowIfNullOrWhiteSpace(rabbitPassword);
            
            h.Username(rabbitUserName);
            h.Password(rabbitPassword);
        });
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

