using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data
{
    public class TreeContext : DbContext
    {

        public TreeContext(DbContextOptions<TreeContext> options) : base(options) { }
        public DbSet<DbNode> Nodes { get; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
                var connectionString = configuration.GetConnectionString("MyConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
