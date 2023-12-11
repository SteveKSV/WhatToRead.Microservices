using Basket.API.Managers;
using Basket.API.Managers.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "http://localhost:5443";
        options.ApiName = "Basket.API";
    });

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
                                {
                                    config.UsingRabbitMq((context, rabbitConfig) => {
                                        rabbitConfig.Host(new Uri(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress")));
                                    });
                                });

builder.Services.AddScoped<IBasketManager, BasketManager>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
