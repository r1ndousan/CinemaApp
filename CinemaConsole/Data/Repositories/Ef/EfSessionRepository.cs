using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaConsole.Data.Repositories;

namespace CinemaConsole.Data.Repositories.Ef
{
    public class EfSessionRepository : ISessionRepository
    {
        private readonly CinemaDbContext _ctx;

        public EfSessionRepository(CinemaDbContext context)
        {
            _ctx = context;
        }

        public async Task AddSessionAsync(Session session)
        {
            await _ctx.Sessions.AddAsync(session);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            return await _ctx.Sessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _ctx.Sessions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateSessionAsync(Session session)
        {
            _ctx.Sessions.Update(session);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var s = await _ctx.Sessions.FindAsync(id);
            if (s is not null)
            {
                _ctx.Sessions.Remove(s);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}

