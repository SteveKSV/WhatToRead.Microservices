using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", true, true);

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
var app = builder.Build();

app.UseRouting();   

await app.UseOcelot();

app.Run();
