using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;

namespace CinemaConsole.Data
{
    public class ClientRepository : IClientRepository
    {
        private readonly CinemaDbContext _dbContext;

        public ClientRepository(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddClientAsync(Client client)
        {
            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            _dbContext.Clients.Update(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await GetClientByIdAsync(clientId);
            if (client != null)
            {
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            return await _dbContext.Clients.FindAsync(clientId);
        }

        public async Task<Client?> GetClientByLoginAsync(string login)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.Login == login);
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        Task<IEnumerable<Client>> IClientRepository.GetAllClientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> FindClientsAsync(Func<Client, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

