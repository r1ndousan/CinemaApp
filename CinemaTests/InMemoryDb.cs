using CinemaConsole.Data;
using Microsoft.EntityFrameworkCore;

public static class InMemoryDb
{
    public static CinemaDbContext Create()
    {
        var opts = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new CinemaDbContext(opts);
    }
}