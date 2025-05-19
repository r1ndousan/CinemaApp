using System;
using System.Collections.Generic;
using System.Windows;
using CinemaUI.Dialogs;
using CinemaUI.Models;
using CinemaUI.Services;
using CinemaUI.Services.CinemaUI.Services;
using Microsoft.VisualBasic;

namespace CinemaUI
{
    public partial class MainWindow : Window
    {
        private readonly ClientService _clientService;
        private readonly SessionService _sessionService;
        private readonly UserService _userService;
        private readonly BookingService _bookingService;

        public MainWindow()
        {
            InitializeComponent();

            var auth = new AuthService(new ApiClient());
            var loginWin = new LoginWindow(auth);
            if (loginWin.ShowDialog() != true)
            {
                Close();
                return;
            }

            var jwt = loginWin.JwtToken!;
            var api = new ApiClient(jwt);

            _clientService = new ClientService(api);
            _sessionService = new SessionService(api);
            _userService = new UserService(api);
            _bookingService = new BookingService(api);

            // первичная загрузка
            _ = LoadClientsAsync();
            _ = LoadSessionsAsync();
            _ = LoadUsersAsync();
            _ = LoadBookingsAsync();
        }

        // ==== Load методы ====

        private async System.Threading.Tasks.Task LoadClientsAsync()
        {
            var items = await _clientService.GetAllAsync();
            ClientsGrid.ItemsSource = items;
        }

        private async System.Threading.Tasks.Task LoadSessionsAsync()
        {
            var items = await _sessionService.GetAllAsync();
            SessionsGrid.ItemsSource = items;
        }

        private async System.Threading.Tasks.Task LoadUsersAsync()
        {
            var items = await _userService.GetAllAsync();
            UsersGrid.ItemsSource = items;
        }

        private async System.Threading.Tasks.Task LoadBookingsAsync()
        {
            var items = await _bookingService.GetAllAsync();
            BookingsGrid.ItemsSource = items;
        }

        // ==== Refresh кнопки ====

        private async void RefreshClients_Click(object s, RoutedEventArgs e) => await LoadClientsAsync();
        private async void RefreshSessions_Click(object s, RoutedEventArgs e) => await LoadSessionsAsync();
        private async void RefreshUsers_Click(object s, RoutedEventArgs e) => await LoadUsersAsync();
        private async void RefreshBookings_Click(object s, RoutedEventArgs e) => await LoadBookingsAsync();

        // ==== Add кнопки ====

        private async void AddClient_Click(object s, RoutedEventArgs e)
        {
            var dto = new ClientDto { Name = "NewClient", Login = "new", PasswordHash = "hash" };
            await _clientService.CreateAsync(dto);
            await LoadClientsAsync();
        }

        private async void AddSession_Click(object s, RoutedEventArgs e)
        {
            var dto = new SessionDto
            {
                StartTime = DateTime.Now.AddHours(1),
                MovieTitle = "New Movie",
                AvailableSeats = 100
            };
            await _sessionService.CreateAsync(dto);
            await LoadSessionsAsync();
        }

        private async void AddUser_Click(object s, RoutedEventArgs e)
        {
            var dto = new UserDto { Username = "newuser", PasswordHash = "hash", Role = "User" };
            await _userService.CreateAsync(dto);
            await LoadUsersAsync();
        }

        private async void AddBooking_Click(object s, RoutedEventArgs e)
        {
            // Проверяем выбранного клиента
            if (ClientsGrid.SelectedItem is not ClientDto client)
            {
                MessageBox.Show("Сначала выберите клиента!");
                return;
            }
            // Проверяем выбранный сеанс
            if (SessionsGrid.SelectedItem is not SessionDto session)
            {
                MessageBox.Show("Сначала выберите сеанс!");
                return;
            }

            // Вводим число билетов
            string input = Interaction.InputBox(
                "Сколько билетов хотите забронировать?",
                "Новая бронь",
                "1"                // значение по умолчанию
            );

            if (!int.TryParse(input, out int seats) || seats <= 0)
            {
                MessageBox.Show("Неверное количество билетов.");
                return;
            }

            var dto = new BookingDto
            {
                ClientId = client.Id,
                SessionId = session.Id,
                SeatsBooked = seats,
                BookingTime = DateTime.Now
            };

            var response = await _bookingService.CreateAsync(dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка при создании бронирования:\n{response.StatusCode}\n{error}");
            }
            else
            {
                await LoadBookingsAsync();
            }
        }



        // ==== Edit/Delete ====

        private async void EditClient_Click(object s, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not ClientDto sel) return;
            var copy = new ClientDto { Id = sel.Id, Name = sel.Name, Login = sel.Login, PasswordHash = sel.PasswordHash };
            var dlg = new ClientEditWindow(copy);
            if (dlg.ShowDialog() == true)
            {
                await _clientService.UpdateAsync(copy.Id, copy);
                await LoadClientsAsync();
            }
        }

        private async void DeleteClient_Click(object s, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is not ClientDto sel) return;
            await _clientService.DeleteAsync(sel.Id);
            await LoadClientsAsync();
        }

        private async void EditSession_Click(object s, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is not SessionDto sel) return;
            var copy = new SessionDto
            {
                Id = sel.Id,
                StartTime = sel.StartTime,
                MovieTitle = sel.MovieTitle,
                AvailableSeats = sel.AvailableSeats
            };
            var dlg = new SessionEditWindow(copy);
            if (dlg.ShowDialog() == true)
            {
                await _sessionService.UpdateAsync(copy.Id, copy);
                await LoadSessionsAsync();
            }
        }

        private async void DeleteSession_Click(object s, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is not SessionDto sel) return;
            await _sessionService.DeleteAsync(sel.Id);
            await LoadSessionsAsync();
        }

        private async void EditUser_Click(object s, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not UserDto sel) return;
            var copy = new UserDto { Id = sel.Id, Username = sel.Username, PasswordHash = sel.PasswordHash, Role = sel.Role };
            var dlg = new UserEditWindow(copy);
            if (dlg.ShowDialog() == true)
            {
                await _userService.UpdateAsync(copy.Id, copy);
                await LoadUsersAsync();
            }
        }

        private async void DeleteUser_Click(object s, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not UserDto sel) return;
            await _userService.DeleteAsync(sel.Id);
            await LoadUsersAsync();
        }

        private async void EditBooking_Click(object s, RoutedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is not BookingDto sel) return;
            var copy = new BookingDto
            {
                Id = sel.Id,
                ClientId = sel.ClientId,
                SessionId = sel.SessionId,
                SeatsBooked = sel.SeatsBooked,
                BookingTime = sel.BookingTime
            };
            var dlg = new BookingEditWindow(copy);
            if (dlg.ShowDialog() == true)
            {
                await _bookingService.UpdateAsync(copy.Id, copy);
                await LoadBookingsAsync();
            }
        }

        private async void DeleteBooking_Click(object s, RoutedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is not BookingDto sel) return;
            await _bookingService.DeleteAsync(sel.Id);
            await LoadBookingsAsync();
        }
    }
}
