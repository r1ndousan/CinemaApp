using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaUI.Models;   // создадим модель DTO

namespace CinemaUI.Services
{
    public class SessionService
    {
        private readonly HttpClient _http;

        public SessionService(ApiClient api)
        {
            _http = api.Client;
        }

        public Task<List<SessionDto>> GetAllAsync() =>
            _http.GetFromJsonAsync<List<SessionDto>>("sessions")!;

        public Task<SessionDto?> GetByIdAsync(int id) =>
            _http.GetFromJsonAsync<SessionDto>($"sessions/{id}");

        public Task<HttpResponseMessage> CreateAsync(SessionDto dto) =>
            _http.PostAsJsonAsync("sessions", dto);

        public Task<HttpResponseMessage> UpdateAsync(int id, SessionDto dto) =>
            _http.PutAsJsonAsync($"sessions/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(int id) =>
            _http.DeleteAsync($"sessions/{id}");
        public Task<List<SessionDto>> FindAsync(DateTime? from, DateTime? to, string movie)
        {
            var qs = new List<string>();
            if (from.HasValue) qs.Add("from=" + Uri.EscapeDataString(from.Value.ToString("o")));
            if (to.HasValue) qs.Add("to=" + Uri.EscapeDataString(to.Value.ToString("o")));
            if (!string.IsNullOrWhiteSpace(movie)) qs.Add("movie=" + Uri.EscapeDataString(movie));
            var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";
            return _http.GetFromJsonAsync<List<SessionDto>>($"sessions{query}")!;
        }
    }
}

