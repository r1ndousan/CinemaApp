using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;

namespace CinemaConsole.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с бронированиями.
    /// </summary>
    public interface IBookingRepository
    {
        Task AddBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
    }
}
