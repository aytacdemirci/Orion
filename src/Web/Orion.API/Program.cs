using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Orion.API;
using Orion.API.CustomMiddlewares;
using Orion.API.SeedWork.Extensions;
using Orion.Application;
using Orion.CosmosRepository;
using Orion.SQLRepository;
using Orion.SQLRepository.StoryRepositories;
using Orion.ThirdPartyServices;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

//Add services to the container.
builder.Services.ApiConfigureDependencyInjection(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMSSQLRepository(builder.Configuration);
//builder.Services.AddCosmosRepository(builder.Configuration);
builder.Services.AddThirdPartyServices(builder.Configuration);
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orion.API", Version = "v1" });
});

builder.Services.ConfigureHealthChecks(builder.Configuration);

var app = builder.Build();

//Auto Migration
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
    db.Database.Migrate();
}

//Add serilog
var elasticsearchUrl = builder.Configuration["Elastic"]; Serilog.Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
              .MinimumLevel.Override("System", LogEventLevel.Debug)
              .WriteTo.Debug()
              .WriteTo.Console()
              .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
              {
                  AutoRegisterTemplate = true,
                  AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                  IndexFormat = $"serverlog-{DateTime.Now:yyyy.MM.dd}"
              })
              .CreateLogger();
// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlerAndLogMiddleware>(app.Environment);

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orion.API v1"));

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    //HealthCheck middleware
    endpoints.MapHealthChecks("/api/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHealthChecksUI(delegate (Options options)
    {
        options.UIPath = "/healthcheck-ui";
        options.AddCustomStylesheet("./HealthChecks/Custom.css");
    });
});

app.Run();

public partial class Program
{
    // Expose the program class for use in integration test project with webapplicationfactory<T>
}

