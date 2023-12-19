using Catalog.Managers.Interfaces;
using Catalog.Managers;
using Catalog;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Amazon.SecurityToken.Model.Internal.MarshallTransformations;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//////////////////////// UseElasticSearch /////////////////////////////////

ConfigureLogging();
builder.Host.UseSerilog();
//////////////////////// DATABASE CONFIGURATION ///////////////////////////////
builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddTransient<MongoDbContext>();

//////////////////////// MANAGERS CONFIGURATION ///////////////////////////////
builder.Services.AddScoped<IBookManager, BookManager>();

//////////////////////// CONTROLLERS CONFIGURATION ///////////////////////////////
builder.Services.AddControllers();

//////////////////////// IDENTITY CONFIGURATION ///////////////////////////////
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:5443";
        options.ApiName = "Catalog.API";
        options.RequireHttpsMetadata = false;
    });

//////////////////////// SERILOG CONFIGURATION ///////////////////////////////
//builder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.Enrich.FromLogContext()
//                 .Enrich.WithMachineName()
//                 .WriteTo.Console()
//                 .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
//                 {
//                     IndexFormat = $"applogs -{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-logs -{DateTime.UtcNow:yyyy-MM}",
//                     AutoRegisterTemplate = true,
//                     NumberOfShards = 2,
//                     NumberOfReplicas = 1,
//                 })
//                  .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
//                  .ReadFrom.Configuration(context.Configuration);

//});

//////////////////////// SWAGGER CONFIGURATION ///////////////////////////////
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
}

app.UseSerilogRequestLogging();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

    Log.Logger = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .Enrich.WithMachineName()
                 .WriteTo.Debug()
                 .WriteTo.Console()
                 .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
                 {

                     IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{environment.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                     AutoRegisterTemplate = true,
                 })
                 .CreateLogger();

}