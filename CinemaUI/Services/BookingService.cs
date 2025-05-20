using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaUI.Models;   // создадим модель DTO

namespace CinemaUI.Services
{
    public class BookingService
    {
        private readonly HttpClient _http;
        public BookingService(ApiClient api)
        {
            _http = api.Client;
        }

        public Task<List<BookingDto>> GetAllAsync() =>
            _http.GetFromJsonAsync<List<BookingDto>>("bookings")!;

        public Task<BookingDto?> GetByIdAsync(int id) =>
            _http.GetFromJsonAsync<BookingDto>($"bookings/{id}");

        public Task<HttpResponseMessage> CreateAsync(BookingDto dto) =>
            _http.PostAsJsonAsync("bookings", dto);

        public Task<HttpResponseMessage> UpdateAsync(int id, BookingDto dto) =>
            _http.PutAsJsonAsync($"bookings/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(int id)
        {
            // Убираем лишний слэш, используем именно bookings/{id}
            return _http.DeleteAsync($"bookings/{id}");
        }
        public Task<List<BookingDto>> FindAsync(int? clientId, int? sessionId)
        {
            var qs = new List<string>();
            if (clientId.HasValue) qs.Add("clientId=" + clientId.Value);
            if (sessionId.HasValue) qs.Add("sessionId=" + sessionId.Value);
            var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";
            return _http.GetFromJsonAsync<List<BookingDto>>($"bookings{query}")!;
        }
    }
}
