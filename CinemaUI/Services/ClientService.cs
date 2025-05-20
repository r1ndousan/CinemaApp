using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaUI.Models;   // создадим модель DTO
namespace CinemaUI.Services
{
    public class ClientService
    {
        private readonly HttpClient _http;

        public ClientService(ApiClient api)
        {
            _http = api.Client;
        }

        public Task<List<ClientDto>> GetAllAsync() =>
            _http.GetFromJsonAsync<List<ClientDto>>("clients")!;

        public Task<ClientDto?> GetByIdAsync(int id) =>
            _http.GetFromJsonAsync<ClientDto>($"clients/{id}");

        public Task<HttpResponseMessage> CreateAsync(ClientDto dto) =>
            _http.PostAsJsonAsync("clients", dto);

        public Task<HttpResponseMessage> UpdateAsync(int id, ClientDto dto) =>
            _http.PutAsJsonAsync($"clients/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(int id) =>
            _http.DeleteAsync($"clients/{id}");

        public Task<List<ClientDto>> FindAsync(string name, string login) =>
    _http.GetFromJsonAsync<List<ClientDto>>(
      $"clients?name={Uri.EscapeDataString(name)}&login={Uri.EscapeDataString(login)}")!;
    }
}

