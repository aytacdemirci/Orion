namespace Orion.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApiConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            return services;
        }
    }
}
