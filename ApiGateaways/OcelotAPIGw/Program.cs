using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var configFileName = $"ocelot.{environment.ToLower()}.json";
builder.Configuration.AddJsonFile(configFileName, true, true);

var authenticationProviderKey = "IdentityApiKey";

//builder.Services.AddAuthentication("Bearer")
//    .AddIdentityServerAuthentication("IdentityApiKey", options =>
//    {
//        options.Authority = "https://localhost:5443";
//        options.ApiName = "OcelotApiGw";
//        options.RequireHttpsMetadata = false;
//    });

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.UseRouting();

await app.UseOcelot();

app.Run();
