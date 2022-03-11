using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orion.Application.StoryAppLayer.Gateway;
using Orion.CosmosRepository.Settings;
using Orion.CosmosRepository.StoryRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.CosmosRepository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCosmosRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosSettings = new CosmosSettings();
            configuration.GetSection(CosmosSettings.SettingName).Bind(cosmosSettings);
            services.AddSingleton(cosmosSettings);

            services.AddSingleton<IStoryCosmosContext, StoryCosmosContext>();

            services.AddScoped<IStoryRepository, StoryRepository>();

            return services;
        }
    }
}