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
    public partial class SessionEditWindow : Window
    {
        public SessionDto Session { get; }

        public SessionEditWindow(SessionDto existing)
        {
            InitializeComponent();
            Session = existing;
            DatePicker.SelectedDate = existing.StartTime.Date;
            TimeBox.Text = existing.StartTime.ToString("HH:mm");
            TitleBox.Text = existing.MovieTitle;
            SeatsBox.Text = existing.AvailableSeats.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate == null ||
                !TimeSpan.TryParse(TimeBox.Text, out var time) ||
                !int.TryParse(SeatsBox.Text, out var seats))
            {
                MessageBox.Show("Неверные данные");
                return;
            }

            Session.StartTime = DatePicker.SelectedDate.Value + time;
            Session.MovieTitle = TitleBox.Text;
            Session.AvailableSeats = seats;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
