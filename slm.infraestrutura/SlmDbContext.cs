
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Slm.Domain.Entidades;

namespace slm.infraestrutura
{
    // classe SlmDbContext herda de DbContext, que é a classe principal do Entity Framework.
    // representa uma sessão com o banco de dados.
    public class SlmDbContext : DbContext
    {
        private readonly IConfiguration _configuration;


        // Cada DbSet mapeia uma entidade para uma tabela.
        public DbSet<Moto> Moto { get; set; }
        public DbSet<Entregador> Entregador { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        // Construtor que recebe a configuração e as opções do DbContext por injeção de dependência.
        public SlmDbContext(IConfiguration configuration, DbContextOptions<SlmDbContext> options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

          
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                Database.Migrate();
            }
        }

        //  método é usado para configurar a conexão com o banco de dados.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Checa se a conexão já foi configurada.
            if (!optionsBuilder.IsConfigured)
            {
                // Lê a variável 'TypeDatabase' do json
                var typeDatabase = _configuration["TypeDatabase"];
                // Obtém a string de conexão completa com base no tipo de banco.
                var connectionString = _configuration.GetConnectionString(typeDatabase);

                if (typeDatabase == "Postgresql")
                {
                    // Usa o provedor Npgsql para configurar a conexão.
                    optionsBuilder.UseNpgsql(connectionString,
                        b => b.MigrationsAssembly("slm.infraestrutura"));
                }
            }
        }
    }
}