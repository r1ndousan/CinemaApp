using CinemaUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaUI.Dialogs
{
    public partial class BookingEditWindow : Window
    {
        public BookingDto Booking { get; private set; }

        public BookingEditWindow(BookingDto existing)
        {
            InitializeComponent();
            Booking = existing;
            ClientIdBox.Text = existing.ClientId.ToString();
            SessionIdBox.Text = existing.SessionId.ToString();
            SeatsBox.Text = existing.SeatsBooked.ToString();
            DatePicker.SelectedDate = existing.BookingTime.Date;
            TimeBox.Text = existing.BookingTime.ToString("HH:mm");
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(ClientIdBox.Text, out var cid) ||
                !int.TryParse(SessionIdBox.Text, out var sid) ||
                !int.TryParse(SeatsBox.Text, out var seats) ||
                DatePicker.SelectedDate == null ||
                !TimeSpan.TryParse(TimeBox.Text, out var time))
            {
                MessageBox.Show("Неверные данные");
                return;
            }

            Booking.ClientId = cid;
            Booking.SessionId = sid;
            Booking.SeatsBooked = seats;
            Booking.BookingTime = DatePicker.SelectedDate.Value + time;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
