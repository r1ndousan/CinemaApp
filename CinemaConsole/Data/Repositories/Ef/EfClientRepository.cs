using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaConsole.Data.Repositories.Ef
{
    public class EfClientRepository : IClientRepository
    {
        private readonly CinemaDbContext _ctx;

        public EfClientRepository(CinemaDbContext context) => _ctx = context;

        public async Task AddClientAsync(Client entity)
        {
            await _ctx.Clients.AddAsync(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _ctx.Clients.FindAsync(id);
            if (client is null) return;
            _ctx.Clients.Remove(client);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Client>> FindClientsAsync(Func<Client, bool> predicate)
        {
            // для простоты: материализуем всю коллекцию
            var all = await _ctx.Clients.ToListAsync();
            return all.Where(predicate);
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _ctx.Clients.AsNoTracking().ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _ctx.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client?> GetClientByLoginAsync(string login)
        {
            return await _ctx.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Login == login);
        }

        public async Task UpdateClientAsync(Client entity)
        {
            _ctx.Clients.Update(entity);
            await _ctx.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<Client>> FindClientsAsync(string? nameFilter, string? loginFilter)
        {
            IQueryable<Client> q = _ctx.Clients.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nameFilter))
                q = q.Where(c => c.Name.Contains(nameFilter));

            if (!string.IsNullOrWhiteSpace(loginFilter))
                q = q.Where(c => c.Login.Contains(loginFilter));

            return await q.ToListAsync();
        }
    }
}
