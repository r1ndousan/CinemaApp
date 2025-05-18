using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaConsole.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Login { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        // Здесь в будущем можно добавить навигационные свойства,
        // например: public ICollection<Booking> Bookings { get; set; }
    }
}