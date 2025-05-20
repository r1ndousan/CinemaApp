using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaConsole.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public int SeatsBooked { get; set; }
        public DateTime BookingTime { get; set; }
    }

}