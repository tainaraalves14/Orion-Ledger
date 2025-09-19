using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OrionLedger.Infrastructure.Data
{
    public class OrionLedgerDbContextFactory : IDesignTimeDbContextFactory<OrionLedgerDbContext>
    {
        public OrionLedgerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrionLedgerDbContext>();
            
            // Constrói a configuração para ler a string de conexão de appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new OrionLedgerDbContext(optionsBuilder.Options);
        }
    }
}