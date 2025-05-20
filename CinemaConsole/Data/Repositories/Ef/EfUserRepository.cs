using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaConsole.Data.Repositories.Ef
{
    public class EfUserRepository : IUserRepository
    {
        private readonly CinemaDbContext _ctx;

        public EfUserRepository(CinemaDbContext context)
        {
            _ctx = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _ctx.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var u = await _ctx.Users.FindAsync(id);
            if (u is not null)
            {
                _ctx.Users.Remove(u);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
