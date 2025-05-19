using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;

namespace CinemaConsole.Data.Repositories.Sql
{
    public class SqlSessionRepository : ISessionRepository
    {
        private readonly string _cs;
        public SqlSessionRepository(string connectionString) => _cs = connectionString;

        public async Task AddSessionAsync(Session session)
        {
            const string sql = @"
                INSERT INTO Sessions (StartTime, MovieTitle, AvailableSeats)
                VALUES (@Time, @Title, @Seats);";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Time", session.StartTime);
            cmd.Parameters.AddWithValue("@Title", session.MovieTitle);
            cmd.Parameters.AddWithValue("@Seats", session.AvailableSeats);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, StartTime, MovieTitle, AvailableSeats
                  FROM Sessions
                 WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync()) return null;
            return new Session
            {
                Id = rdr.GetInt32(0),
                StartTime = rdr.GetDateTime(1),
                MovieTitle = rdr.GetString(2),
                AvailableSeats = rdr.GetInt32(3)
            };
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            const string sql = @"
                SELECT Id, StartTime, MovieTitle, AvailableSeats
                  FROM Sessions;";
            var list = new List<Session>();
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Session
                {
                    Id = rdr.GetInt32(0),
                    StartTime = rdr.GetDateTime(1),
                    MovieTitle = rdr.GetString(2),
                    AvailableSeats = rdr.GetInt32(3)
                });
            }
            return list;
        }

        public async Task UpdateSessionAsync(Session session)
        {
            const string sql = @"
                UPDATE Sessions
                   SET StartTime      = @Time,
                       MovieTitle     = @Title,
                       AvailableSeats = @Seats
                 WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", session.Id);
            cmd.Parameters.AddWithValue("@Time", session.StartTime);
            cmd.Parameters.AddWithValue("@Title", session.MovieTitle);
            cmd.Parameters.AddWithValue("@Seats", session.AvailableSeats);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteSessionAsync(int id)
        {
            const string sql = "DELETE FROM Sessions WHERE Id = @Id;";
            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
