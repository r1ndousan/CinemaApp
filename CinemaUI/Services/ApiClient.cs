using System;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace CinemaUI.Services
{
    public class ApiClient
    {
        public HttpClient Client { get; }

        public ApiClient()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
        }
    }
}

