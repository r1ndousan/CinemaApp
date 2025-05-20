using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;      // <-- пакет Microsoft.Data.SqlClient
using CinemaConsole.Data.Entities;
using System.Text;

namespace CinemaConsole.Data.Repositories.Sql
{
    public class SqlClientRepository : IClientRepository
    {
        private readonly string _cs;
        public SqlClientRepository(string connectionString) => _cs = connectionString;

        public async Task AddClientAsync(Client client)
        {
            const string sql = @"
                INSERT INTO Clients (Name, Login, PasswordHash)
                VALUES (@Name, @Login, @Hash);";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Login", client.Login);
            cmd.Parameters.AddWithValue("@Hash", client.PasswordHash);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<IReadOnlyList<Client>> FindClientsAsync(string? nameFilter, string? loginFilter)
        {
            var sql = new StringBuilder("SELECT Id, Name, Login, PasswordHash FROM Clients WHERE 1=1");
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                sql.Append(" AND Name LIKE @name");
                parameters.Add(new SqlParameter("@name", $"%{nameFilter}%"));
            }
            if (!string.IsNullOrWhiteSpace(loginFilter))
            {
                sql.Append(" AND Login LIKE @login");
                parameters.Add(new SqlParameter("@login", $"%{loginFilter}%"));
            }

            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(parameters.ToArray());

            await conn.OpenAsync();
            var list = new List<Client>();
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    Login = rdr.GetString(2),
                    PasswordHash = rdr.GetString(3)
                });
            }

            return list;
        }
        public async Task<Client?> GetClientByIdAsync(int id)
        {
            const string sql = "SELECT Id, Name, Login, PasswordHash FROM Clients WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync()) return null;
            return new Client
            {
                Id = rdr.GetInt32(0),
                Name = rdr.GetString(1),
                Login = rdr.GetString(2),
                PasswordHash = rdr.GetString(3)
            };
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            const string sql = "SELECT Id, Name, Login, PasswordHash FROM Clients;";
            var list = new List<Client>();
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    Login = rdr.GetString(2),
                    PasswordHash = rdr.GetString(3)
                });
            }
            return list;
        }

        public async Task<IEnumerable<Client>> FindClientsAsync(Func<Client, bool> predicate)
        {
            var all = await GetAllClientsAsync();
            return all.Where(predicate);
        }

        public async Task UpdateClientAsync(Client client)
        {
            const string sql = @"
                UPDATE Clients
                   SET Name = @Name,
                       Login = @Login,
                       PasswordHash = @Hash
                 WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", client.Id);
            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Login", client.Login);
            cmd.Parameters.AddWithValue("@Hash", client.PasswordHash);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            const string sql = "DELETE FROM Clients WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Client?> GetClientByLoginAsync(string login)
        {
            const string sql = "SELECT Id, Name, Login, PasswordHash FROM Clients WHERE Login = @Login;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Login", login);
            await conn.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync()) return null;
            return new Client
            {
                Id = rdr.GetInt32(0),
                Name = rdr.GetString(1),
                Login = rdr.GetString(2),
                PasswordHash = rdr.GetString(3)
            };
        }
    }


}
