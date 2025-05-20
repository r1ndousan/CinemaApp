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

        public MainWindow() : this(string.Empty)
        {
        }
        // Новый конструктор, принимающий JWT
        public MainWindow(string jwt)
        {
            InitializeComponent();

            var api = new ApiClient(jwt);
            _clientService = new ClientService(api);
            _sessionService = new SessionService(api);
            _userService = new UserService(api);
            _bookingService = new BookingService(api);

            _ = LoadClientsAsync();
            _ = LoadSessionsAsync();
            _ = LoadBookingsAsync();
        }
     

        // ==== Refresh кнопки ====

        private async void RefreshClients_Click(object s, RoutedEventArgs e) => await LoadClientsAsync();
        private async void RefreshSessions_Click(object s, RoutedEventArgs e) => await LoadSessionsAsync();

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

        private async Task LoadClientsAsync()
        {
            var name = ClientNameFilter.Text;
            var login = ClientLoginFilter.Text;
            List<ClientDto> list;
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(login))
            {
                // Без фильтра — получаем всех
                list = await _clientService.GetAllAsync();
            }
            else
            {
                // С фильтром
                list = await _clientService.FindAsync(name, login);
            }

            ClientsGrid.ItemsSource = list;
        }

        private void ResetClientsFilter_Click(object s, RoutedEventArgs e)
        {
            ClientNameFilter.Clear();
            ClientLoginFilter.Clear();
            _ = LoadClientsAsync();
        }

        // Сеансы
        private async Task LoadSessionsAsync()
        {
            var from = SessionFromFilter.SelectedDate;
            var to = SessionToFilter.SelectedDate;
            var movie = SessionMovieFilter.Text;

            List<SessionDto> list;
            if (from == null && to == null && string.IsNullOrWhiteSpace(movie))
                list = await _sessionService.GetAllAsync();
            else
                list = await _sessionService.FindAsync(from, to, movie);

            SessionsGrid.ItemsSource = list;
        }

        private void ResetSessionsFilter_Click(object s, RoutedEventArgs e)
        {
            SessionFromFilter.SelectedDate = null;
            SessionToFilter.SelectedDate = null;
            SessionMovieFilter.Clear();
            _ = LoadSessionsAsync();
        }


        // Бронирования
        private async Task LoadBookingsAsync()
        {
            int? cid = int.TryParse(BookingClientFilter.Text, out var a) ? a : null;
            int? sid = int.TryParse(BookingSessionFilter.Text, out var b) ? b : null;

            List<BookingDto> list;
            if (cid == null && sid == null)
                list = await _bookingService.GetAllAsync();
            else
                list = await _bookingService.FindAsync(cid, sid);

            BookingsGrid.ItemsSource = list;
        }

        private void ResetBookingsFilter_Click(object s, RoutedEventArgs e)
        {
            BookingClientFilter.Clear();
            BookingSessionFilter.Clear();
            _ = LoadBookingsAsync();
        }

    }
}
