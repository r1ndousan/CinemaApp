using System;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CinemaUI.Services
{
    public class ApiClient
    {
        public HttpClient Client { get; }

        public ApiClient(string? jwt = null)
        {
            Client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
            if (!string.IsNullOrEmpty(jwt))
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }

}

