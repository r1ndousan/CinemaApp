using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CinemaConsole.Data;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using System.Linq;

namespace CinemaConsole
{
    public static class Demo
    {
        public static async Task RunAsync(IServiceProvider services)
        {
            // 1) Получаем наши репозитории из контейнера
            var clientRepo = services.GetRequiredService<IClientRepository>();
            var sessionRepo = services.GetRequiredService<ISessionRepository>();

            // --- CLIENT CRUD ---
            Console.WriteLine("=== Клиенты ===");

            // Create
            var client = new Client
            {
                Name = "Иван Иванов",
                Login = "ivanov",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("пароль123")
            };
            await clientRepo.AddClientAsync(client);
            Console.WriteLine($"Добавлен клиент Id={client.Id}");

            // Read All
            var allClients = await clientRepo.GetAllClientsAsync();
            Console.WriteLine($"Всего клиентов: {allClients.Count()}");

            // Update
            client.Name = "Иван Петров";
            await clientRepo.UpdateClientAsync(client);
            Console.WriteLine($"Обновлён клиент Id={client.Id}");

            // Read by Id
            var getById = await clientRepo.GetClientByIdAsync(client.Id);
            Console.WriteLine($"Клиент Id={getById?.Id}, Login={getById?.Login}, Name={getById?.Name}");

            // Delete
            await clientRepo.DeleteClientAsync(client.Id);
            Console.WriteLine($"Удалён клиент Id={client.Id}");

            // --- SESSION CRUD ---
            Console.WriteLine("\n=== Сеансы ===");

            // Create
            var session = new Session
            {
                MovieTitle = "Матрица",
                StartTime = DateTime.Now.AddHours(1),
                AvailableSeats = 50
            };
            await sessionRepo.AddSessionAsync(session);
            Console.WriteLine($"Добавлен сеанс Id={session.Id}");

            // Read All
            var allSessions = await sessionRepo.GetAllSessionsAsync();
            Console.WriteLine($"Всего сеансов: {allSessions.Count}");

            // Update
            session.AvailableSeats = 45;
            await sessionRepo.UpdateSessionAsync(session);
            Console.WriteLine($"Обновлён сеанс Id={session.Id}");

            // Read by Date
            var todaySessions = await sessionRepo.GetSessionsByDateAsync(DateTime.Today);
            Console.WriteLine($"Сеансов сегодня: {todaySessions.Count}");

            // Delete
            await sessionRepo.DeleteSessionAsync(session.Id);
            Console.WriteLine($"Удалён сеанс Id={session.Id}");
        }
    }
}
