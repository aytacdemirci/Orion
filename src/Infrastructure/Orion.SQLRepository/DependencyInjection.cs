using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orion.Application.StoryAppLayer.Gateway;
using Orion.SQLRepository.StoryRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.SQLRepository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMSSQLRepository(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoryDbContext>(options =>
             options.UseSqlServer(
            configuration.GetConnectionString("MSSQL")));
            services.AddScoped<IStoryRepository, StoryRepository>();
            return services;
        }
    }
}
