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

namespace CinemaUI
{
    public partial class MainWindow : Window
    {
        private readonly ClientService _clientService;

        public MainWindow()
        {
            InitializeComponent();

            var api = new ApiClient();
            _clientService = new ClientService(api);

            // при старте сразу загрузим
            _ = LoadClientsAsync();
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
    }
}