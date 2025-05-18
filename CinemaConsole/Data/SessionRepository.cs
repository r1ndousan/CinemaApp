using System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CinemaDbContext _dbContext;

        public SessionRepository(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSessionAsync(Session session)
        {
            _dbContext.Sessions.Add(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSessionAsync(Session session)
        {
            _dbContext.Sessions.Update(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(int sessionId)
        {
            var session = await GetSessionByIdAsync(sessionId);
            if (session != null)
            {
                _dbContext.Sessions.Remove(session);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Session?> GetSessionByIdAsync(int sessionId)
        {
            return await _dbContext.Sessions.FindAsync(sessionId);
        }

        public async Task<List<Session>> GetSessionsByDateAsync(DateTime date)
        {
            return await _dbContext.Sessions
                .Where(s => s.StartTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<List<Session>> GetAllSessionsAsync()
        {
            return await _dbContext.Sessions.ToListAsync();
        }
    }
}

