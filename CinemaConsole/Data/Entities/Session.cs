using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CinemaConsole.Data.Entities
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required, MaxLength(200)]
        public string MovieTitle { get; set; } = null!;

        [Range(0, 1000)]
        public int AvailableSeats { get; set; }
    }
}
