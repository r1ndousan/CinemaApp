using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.Repositories
{
    public interface ISessionRepository
    {
        Task AddSessionAsync(Session session);
        Task<Session?> GetSessionByIdAsync(int id);
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(int id);
        // При желании: Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date);
    }
}

