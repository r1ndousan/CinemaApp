using CinemaConsole.Data.Entities;
using CinemaConsole.Data.IEntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CinemaConsole.Data
{
    public class CinemaDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        private readonly string? _connectionString;

        // Конструктор для рантайма через DI
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        // Ваш «ручной» конструктор
        public CinemaDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // если мы пришли через конструктор со строкой
                optionsBuilder.UseSqlServer(_connectionString!);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
        }

    }
}
