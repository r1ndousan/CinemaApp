using CinemaConsole.Data.Entities;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.Repositories
{
    public interface IClientRepository
    {
        Task AddClientAsync(Client client);
        Task<Client?> GetClientByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<Client>> FindClientsAsync(Func<Client, bool> predicate);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
        Task<Client?> GetClientByLoginAsync(string login);
    }
}
