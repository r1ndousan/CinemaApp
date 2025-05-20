using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories.Ef;
using FluentAssertions;

namespace ClientTests
{
public class EfClientRepositoryTests
{
    [Fact]
    public async Task AddClient_Then_GetById_ReturnsClient()
    {
        await using var ctx = InMemoryDb.Create();
        var repo = new EfClientRepository(ctx);

        // Задаём обязательное PasswordHash
        var c = new Client
        {
            Name = "Иван",
            Login = "ivan",
            PasswordHash = "somehash"   // <- здесь ненулевое значение
        };

        await repo.AddClientAsync(c);

        var fetched = await repo.GetClientByIdAsync(c.Id);
        fetched.Should().NotBeNull();
        fetched!.Login.Should().Be("ivan");
        fetched.PasswordHash.Should().Be("somehash");
    }

    [Fact]
    public async Task FindClientsAsync_FiltersByName()
    {
        await using var ctx = InMemoryDb.Create();
        var repo = new EfClientRepository(ctx);

        // При создании клиентов тоже не забываем PasswordHash
        await repo.AddClientAsync(new Client
        {
            Name = "Alice",
            Login = "a",
            PasswordHash = "h1"
        });
        await repo.AddClientAsync(new Client
        {
            Name = "Bob",
            Login = "b",
            PasswordHash = "h2"
        });

        var list = await repo.FindClientsAsync("Ali", null);
        list.Should().ContainSingle().Which.Name.Should().Be("Alice");
    }
        [Fact]
        public async Task DeleteClient_RemovesClientFromDatabase()
        {
            await using var ctx = InMemoryDb.Create();
            var repo = new EfClientRepository(ctx);

            var client = new Client { Name = "John", Login = "john_doe", PasswordHash = "12345" };
            await repo.AddClientAsync(client);

            var fetchedClient = await repo.GetClientByIdAsync(client.Id);
            fetchedClient.Should().NotBeNull();

            await repo.DeleteClientAsync(client.Id);

            fetchedClient = await repo.GetClientByIdAsync(client.Id);
            fetchedClient.Should().BeNull();
        }

    }

}
