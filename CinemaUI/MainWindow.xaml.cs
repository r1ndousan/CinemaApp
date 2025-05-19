using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaUI.Models;
using CinemaUI.Services;
using System.Threading.Tasks;

namespace CinemaUI
{
    public partial class MainWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly SessionService _sessionService;

        public MainWindow()
        {
            InitializeComponent();
            var api = new ApiClient();
            _clientService = new ClientService(api);
            _sessionService = new SessionService(api);

            _ = LoadClientsAsync();
            _ = LoadSessionsAsync();
        }

        private async Task LoadSessionsAsync()
        {
            List<SessionDto> list = await _sessionService.GetAllAsync();
            SessionsGrid.ItemsSource = list;
        }

        private async Task LoadClientsAsync()
        {
            var list = await _clientService.GetAllAsync();
            ClientsGrid.ItemsSource = list;
        }

        private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
            => await LoadClientsAsync();

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // для примера просто добавим тестового
            var dto = new ClientDto { Name = "Test", Login = "test", PasswordHash = "hash" };
            await _clientService.CreateAsync(dto);
            await LoadClientsAsync();
        }

        // Клиенты
        private async void RefreshClients_Click(object sender, RoutedEventArgs e)
            => await LoadClientsAsync();

        private async void AddClient_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно показать диалог или просто тестовую вставку:
            var dto = new ClientDto { Name = "New", Login = "new", PasswordHash = "hash" };
            await _clientService.CreateAsync(dto);
            await LoadClientsAsync();
        }

        // Сеансы
        private async void RefreshSessions_Click(object sender, RoutedEventArgs e)
            => await LoadSessionsAsync();

        private async void AddSession_Click(object sender, RoutedEventArgs e)
        {
            var dto = new SessionDto
            {
                StartTime = DateTime.Now.AddHours(1),
                MovieTitle = "Новый фильм",
                AvailableSeats = 50
            };
            await _sessionService.CreateAsync(dto);
            await LoadSessionsAsync();
        }
    }

}
