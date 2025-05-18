using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaConsole.Data
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string MovieTitle { get; set; }
        public int AvailableSeats { get; set; }
    }
}
