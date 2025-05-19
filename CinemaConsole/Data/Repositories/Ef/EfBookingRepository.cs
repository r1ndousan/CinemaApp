using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaConsole.Data;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaConsole.Data.Repositories.Ef
{
    public class EfBookingRepository : IBookingRepository
    {
        private readonly CinemaDbContext _ctx;

        public EfBookingRepository(CinemaDbContext context)
        {
            _ctx = context;
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _ctx.Bookings.AddAsync(booking);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Client)
                .Include(b => b.Session)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Client)
                .Include(b => b.Session)
                .ToListAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _ctx.Bookings.Update(booking);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var b = await _ctx.Bookings.FindAsync(id);
            if (b is not null)
            {
                _ctx.Bookings.Remove(b);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
