using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaUI.Models
{

    public class SessionDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public string MovieTitle { get; set; } = "";
        public int AvailableSeats { get; set; }
    }
}
