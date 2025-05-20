using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaUI.Models
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int SessionId { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
