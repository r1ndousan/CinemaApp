using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories.Ef;
using FluentAssertions;

namespace SessionTests
{
    public class EfSessionRepositoryTests
    {
        [Fact]
        public async Task AddSession_Then_GetAll_ReturnsIt()
        {
            await using var ctx = InMemoryDb.Create();
            var repo = new EfSessionRepository(ctx);

            var s = new Session { MovieTitle = "M", StartTime = DateTime.Today, AvailableSeats = 10 };
            await repo.AddSessionAsync(s);

            var all = await repo.GetAllSessionsAsync();
            all.Should().ContainSingle().Which.MovieTitle.Should().Be("M");
        }

        [Fact]
        public async Task FindSessionsAsync_ByDateRange()
        {
            await using var ctx = InMemoryDb.Create();
            var repo = new EfSessionRepository(ctx);

            var t1 = DateTime.Today;
            var t2 = t1.AddDays(1);

            await repo.AddSessionAsync(new Session { MovieTitle = "X", StartTime = t1, AvailableSeats = 5 });
            await repo.AddSessionAsync(new Session { MovieTitle = "Y", StartTime = t2, AvailableSeats = 5 });

            var list = await repo.FindSessionsAsync(t2, null, null);
            list.Should().ContainSingle().Which.MovieTitle.Should().Be("Y");
        }
        [Fact]
        public async Task UpdateSession_ChangesSessionDetails()
        {
            await using var ctx = InMemoryDb.Create();
            var repo = new EfSessionRepository(ctx);

            var session = new Session { MovieTitle = "Old Title", StartTime = DateTime.Today, AvailableSeats = 100 };
            await repo.AddSessionAsync(session);

            session.MovieTitle = "New Title";
            session.AvailableSeats = 50;
            await repo.UpdateSessionAsync(session);

            var fetchedSession = await repo.GetSessionByIdAsync(session.Id);
            fetchedSession.Should().NotBeNull();
            fetchedSession!.MovieTitle.Should().Be("New Title");
            fetchedSession.AvailableSeats.Should().Be(50);
        }
    }
}

