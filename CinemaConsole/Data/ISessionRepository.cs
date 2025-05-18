using CinemaConsole.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace CinemaConsole.Data
{
    public interface ISessionRepository
    {
        Task AddSessionAsync(Session session);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(int sessionId);
        Task<Session?> GetSessionByIdAsync(int sessionId);
        Task<List<Session>> GetSessionsByDateAsync(DateTime date);
        Task<List<Session>> GetAllSessionsAsync();
    }
}