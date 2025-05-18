using System;
using Microsoft.Extensions.Configuration;
using ConsoleAlias = System.Console;

namespace CinemaConsole
{
    internal class Program
    {
        static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            // 1) Настройка конфигурации
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2) Получаем строку подключения
            var connectionString = Configuration.GetConnectionString("CinemaDb");
            Console.WriteLine($"Подключение: {connectionString}");

            // TODO: здесь будем создавать DbContext и дальше работать с БД
        }
    }

}

