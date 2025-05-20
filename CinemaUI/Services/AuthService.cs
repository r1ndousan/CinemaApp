using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaUI.Models;
using System.Net.Http;

namespace CinemaUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        public AuthService(ApiClient api) => _http = api.Client;

        public async Task<AuthResponseDto?> LoginAsync(string username, string password)
        {
            var req = new AuthRequestDto { Username = username, Password = password };
            var resp = await _http.PostAsJsonAsync("auth/login", req);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        public Task<HttpResponseMessage> RegisterAsync(string username, string password)
        {
            var req = new AuthRequestDto { Username = username, Password = password };
            return _http.PostAsJsonAsync("auth/register", req);
        }
    }

}
