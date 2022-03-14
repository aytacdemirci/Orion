using Microsoft.OpenApi.Models;
using Orion.API.CustomMiddlewares;
using Orion.Application;
using Orion.CosmosRepository;
using Orion.SQLRepository;
using Orion.ThirdPartyServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlerMiddleware>(app.Environment);

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orion.API v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    // Expose the program class for use in integration test project with webapplicationfactory<T>
}