using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orion.API.HealthChecks;

namespace Orion.API.SeedWork.Extensions
{
    public static class ServiceExtentions
    {
        //CORS Extention Method
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
        }

        //Health Checks
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            //Sql Server HealthCheck
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:MSSQL"], healthQuery: "select 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Orion", "Database" });

            //Remote Health Check
            services.AddHealthChecks()
                .AddCheck<RemoteHealthCheck>("Remote endpoints Health Check", failureStatus: HealthStatus.Unhealthy);

            //Memory Health Check
            /* services.AddHealthChecks()
                 .AddCheck<MemoryHealthCheck>("Orion Service Memory Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Orion Service" });
            */

            //Uris Health Check
            services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://localhost:5001/api/HeartBeat/ping"), name: "base url", failureStatus: HealthStatus.Unhealthy);

            // UI Health Check
            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); // time in seconds between check
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximim histoy of checks
                opt.SetApiMaxActiveRequests(1);//api requests concurrency
                opt.AddHealthCheckEndpoint("Feedback api", "/api/health");//map health check api

            })
    .AddInMemoryStorage();
        }


    }
}
