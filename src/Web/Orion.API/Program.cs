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

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.ApiConfigureDependencyInjection(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMSSQLRepository(builder.Configuration);
//builder.Services.AddCosmosRepository(builder.Configuration);
builder.Services.AddThirdPartyServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orion.API", Version = "v1" });
});

builder.Services.ConfigureHealthChecks(builder.Configuration);

var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoryDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlerMiddleware>(app.Environment);

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

