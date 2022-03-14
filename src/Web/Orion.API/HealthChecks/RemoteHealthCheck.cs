using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orion.API.HealthChecks
{
    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RemoteHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var httpclient = _httpClientFactory.CreateClient())
            {
                var response = await httpclient.GetAsync("https://api.ipify.org");
                if (!response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Unhealthy("Remote endpoint is unhealthy");
                }

                var response2 = await httpclient.GetAsync("https://www.youtube.com/watch?v=DskTR7SKT7E&t=51s");
                if (!response2.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Unhealthy("Remote endpoint is unhealthy");
                }
                return HealthCheckResult.Healthy($"Remote endpoints is healthy.");
                
            }
        }
    }
}
