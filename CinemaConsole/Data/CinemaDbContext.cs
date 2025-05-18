using Microsoft.EntityFrameworkCore;

namespace CinemaConsole.Data
{
    public class CinemaDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }

        private readonly string _connectionString;

        public CinemaDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // здесь в дальнейшем будут настройки сущностей (например, длина строк, связи и т.д.)
        }
    }
}

