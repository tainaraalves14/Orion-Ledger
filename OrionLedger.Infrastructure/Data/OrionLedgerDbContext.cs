using Microsoft.EntityFrameworkCore;
using OrionLedger.Domain.Entities;

namespace OrionLedger.Infrastructure.Data
{
    public class OrionLedgerDbContext : DbContext
    {
        public OrionLedgerDbContext(DbContextOptions<OrionLedgerDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura chave primária
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

            // Lock otimista (opcional)
            modelBuilder.Entity<User>().Property<byte[]>("RowVersion").IsRowVersion();
        }
    }
}