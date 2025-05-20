using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using System.Text;

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
        public async Task<IReadOnlyList<Session>> FindSessionsAsync(DateTime? from, DateTime? to, string? movieFilter)
        {
            var sql = new StringBuilder("SELECT Id, StartTime, MovieTitle, AvailableSeats FROM Sessions WHERE 1=1");
            var prms = new List<SqlParameter>();

            if (from.HasValue)
            {
                sql.Append(" AND StartTime >= @from");
                prms.Add(new SqlParameter("@from", from.Value));
            }
            if (to.HasValue)
            {
                sql.Append(" AND StartTime <= @to");
                prms.Add(new SqlParameter("@to", to.Value));
            }
            if (!string.IsNullOrWhiteSpace(movieFilter))
            {
                sql.Append(" AND MovieTitle LIKE @mv");
                prms.Add(new SqlParameter("@mv", $"%{movieFilter}%"));
            }

            await using var conn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(prms.ToArray());
            await conn.OpenAsync();

            var list = new List<Session>();
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
    }
}
