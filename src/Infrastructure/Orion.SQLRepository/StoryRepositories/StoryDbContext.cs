using Microsoft.EntityFrameworkCore;
using Orion.Domain.StoryDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orion.SQLRepository.StoryRepositories
{
    public class StoryDbContext : DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> option):base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Story> Story { get; set; }
    }
}
