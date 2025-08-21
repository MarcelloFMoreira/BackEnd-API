using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Slm.Domain.Entidades;

namespace slm.infraestrutura
{
    public class SlmDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Moto> Moto { get; set; }
        public DbSet<Entregador> Entregador { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        public SlmDbContext(IConfiguration configuration, DbContextOptions<SlmDbContext> options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                Database.Migrate();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var typeDatabase = _configuration["TypeDatabase"];
                var connectionString = _configuration.GetConnectionString(typeDatabase);

                if (typeDatabase == "Postgresql")
                {
                    optionsBuilder.UseNpgsql(connectionString, 
                        b => b.MigrationsAssembly("slm.infraestrutura"));
                }
            }
        }
    }
}