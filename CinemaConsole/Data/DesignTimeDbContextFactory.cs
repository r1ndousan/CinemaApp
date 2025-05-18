using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CinemaConsole.Data
{
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<CinemaDbContext>
    {
        public CinemaDbContext CreateDbContext(string[] args)
        {
            // 1) Считываем конфиг
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2) Берём строку
            var connectionString = configuration
                .GetConnectionString("CinemaDb");

            // 3) Настраиваем опции
            var optionsBuilder = new DbContextOptionsBuilder<CinemaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // 4) Возвращаем новый контекст
            return new CinemaDbContext(optionsBuilder.Options);
        }
    }
}
