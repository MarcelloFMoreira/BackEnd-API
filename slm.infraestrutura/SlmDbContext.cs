using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Slm.Domain.Entidades;

namespace slm.infraestrutura
{
    public class SlmDbContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Moto> Moto { get; set; }
        public DbSet<Entregador> Entregador { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        public SlmDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var typeDatabase = _configuration["TypeDatabase"];
            var connectionString = _configuration.GetConnectionString(typeDatabase);


            if (typeDatabase == "Postgresql")
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
