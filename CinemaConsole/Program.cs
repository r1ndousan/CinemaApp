using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CinemaConsole.Data;
using CinemaConsole.Data.Repositories; // пространство ваших репозиториев и DbContext

namespace CinemaConsole
{
    internal class Program
    {
        // Точка входа теперь возвращает Task, чтобы можно было использовать await
        static async Task Main(string[] args)
        {
            // 1) Создаём хост, который настроит нам DI и конфиг
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // указываем, где искать appsettings.json
                    config.SetBasePath(AppContext.BaseDirectory)
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // 2) Получаем строку подключения из конфигурации
                    string cs = hostingContext.Configuration.GetConnectionString("CinemaDb");

                    // 3) Регистрируем DbContext с этой строкой
                    services.AddDbContext<CinemaDbContext>(options =>
                        options.UseSqlServer(cs));

                    // 4) Регистрируем репозитории
                    services.AddScoped<IClientRepository, ClientRepository>();
                    services.AddScoped<ISessionRepository, SessionRepository>();
                })
                .Build();

            // 5) Запускаем демонстрацию (скором создадим Demo.RunAsync)
            await Demo.RunAsync(host.Services);
        }
    }

}

