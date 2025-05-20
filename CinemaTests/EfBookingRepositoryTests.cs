using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories.Ef;
using FluentAssertions;

namespace BookingTests
{
    public class EfBookingRepositoryTests
    {
        [Fact]
        public async Task AddBooking_Then_GetAll_ReturnsIt()
        {
            await using var ctx = InMemoryDb.Create();
            // Нужно предварительно добавить клиента и сеанс, иначе FK-constraint
            ctx.Clients.Add(new Client { Id = 1, Name = "C", Login = "c", PasswordHash = "p" });
            ctx.Sessions.Add(new Session { Id = 1, MovieTitle = "S", StartTime = DateTime.Now, AvailableSeats = 100 });
            await ctx.SaveChangesAsync();

            var repo = new EfBookingRepository(ctx);
            var b = new Booking { ClientId = 1, SessionId = 1, SeatsBooked = 2, BookingTime = DateTime.Now };
            await repo.AddBookingAsync(b);

            var all = await repo.GetAllBookingsAsync();
            all.Should().ContainSingle().Which.SeatsBooked.Should().Be(2);
        }

        [Fact]
        public async Task FindBookingsAsync_FiltersByClientId()
        {
            await using var ctx = InMemoryDb.Create();
            ctx.Clients.Add(new Client { Id = 1, Name = "C", Login = "c", PasswordHash = "p" });
            ctx.Sessions.Add(new Session { Id = 1, MovieTitle = "S", StartTime = DateTime.Now, AvailableSeats = 100 });
            await ctx.SaveChangesAsync();

            var repo = new EfBookingRepository(ctx);
            await repo.AddBookingAsync(new Booking { ClientId = 1, SessionId = 1, SeatsBooked = 1, BookingTime = DateTime.Now });
            await repo.AddBookingAsync(new Booking { ClientId = 2, SessionId = 1, SeatsBooked = 1, BookingTime = DateTime.Now });

            var list = await repo.FindBookingsAsync(1, null);
            list.Should().ContainSingle().Which.ClientId.Should().Be(1);
        }
            [Fact]
            public async Task DeleteBooking_RemovesBookingFromDatabase()
            {
                await using var ctx = InMemoryDb.Create();
                ctx.Clients.Add(new Client { Id = 1, Name = "C", Login = "c", PasswordHash = "p" });
                ctx.Sessions.Add(new Session { Id = 1, MovieTitle = "S", StartTime = DateTime.Now, AvailableSeats = 100 });
                await ctx.SaveChangesAsync();

                var repo = new EfBookingRepository(ctx);

                var booking = new Booking { ClientId = 1, SessionId = 1, SeatsBooked = 2, BookingTime = DateTime.Now };
                await repo.AddBookingAsync(booking);

                var fetchedBooking = await repo.GetBookingByIdAsync(booking.Id);
                fetchedBooking.Should().NotBeNull();

                await repo.DeleteBookingAsync(booking.Id);

                fetchedBooking = await repo.GetBookingByIdAsync(booking.Id);
                fetchedBooking.Should().BeNull();
            }
        }
}

