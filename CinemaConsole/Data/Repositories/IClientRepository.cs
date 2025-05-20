using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.Repositories
{
    /// <summary>
    /// Интерфейс для работы с клиентами.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Добавить нового клиента.
        /// </summary>
        Task AddClientAsync(Client client);

        /// <summary>
        /// Получить клиента по его идентификатору.
        /// </summary>
        Task<Client?> GetClientByIdAsync(int id);

        /// <summary>
        /// Получить всех клиентов.
        /// </summary>
        Task<IEnumerable<Client>> GetAllClientsAsync();

        /// <summary>
        /// Обновить данные клиента.
        /// </summary>
        Task UpdateClientAsync(Client client);

        /// <summary>
        /// Удалить клиента по идентификатору.
        /// </summary>
        Task DeleteClientAsync(int id);

        /// <summary>
        /// Найти клиента по логину.
        /// </summary>
        Task<Client?> GetClientByLoginAsync(string login);
        Task<IReadOnlyList<Client>> FindClientsAsync(string? name, string? login);
    }
}