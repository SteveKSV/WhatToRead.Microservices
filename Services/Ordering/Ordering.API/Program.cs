using Ordering.Infrastructure;
using Ordering.Application;
using EventBus.Messages.Common;
using Ordering.API.EventBusConsumer;
using MassTransit.MultiBus;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, rabbitConfig) => {
        rabbitConfig.Host(new Uri(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress")));
        rabbitConfig.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

builder.Services.AddControllers();

//builder.Services.AddAuthentication("Bearer")
//    .AddIdentityServerAuthentication("Bearer", options =>
//    {
//        options.Authority = "http://localhost:5443";
//        options.ApiName = "Ordering.API";
//    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
