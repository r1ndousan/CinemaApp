using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;

namespace CinemaConsole.Commands
{
    public class AddBookingCommand : ICommand
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly Booking _booking;

        public AddBookingCommand(IBookingRepository bookingRepo, Booking booking)
        {
            _bookingRepo = bookingRepo;
            _booking = booking;
        }

        public async Task ExecuteAsync()
        {
            await _bookingRepo.AddBookingAsync(_booking);
        }
    }
}
