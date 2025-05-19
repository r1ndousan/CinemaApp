using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaUI.Models;   // создадим модель DTO
namespace CinemaUI.Services
{
    namespace CinemaUI.Services
    {
        public class UserService
        {
            private readonly HttpClient _http;
            public UserService(ApiClient api) => _http = api.Client;

            public Task<List<UserDto>> GetAllAsync() =>
                _http.GetFromJsonAsync<List<UserDto>>("users")!;

            public Task<UserDto?> GetByIdAsync(int id) =>
                _http.GetFromJsonAsync<UserDto>($"users/{id}");

            public Task<HttpResponseMessage> CreateAsync(UserDto dto) =>
                _http.PostAsJsonAsync("users", dto);

            public Task<HttpResponseMessage> UpdateAsync(int id, UserDto dto) =>
                _http.PutAsJsonAsync($"users/{id}", dto);

            public Task<HttpResponseMessage> DeleteAsync(int id) =>
                _http.DeleteAsync($"users/{id}");
        }
    }
}

