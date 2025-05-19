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
using CinemaUI.Services;
using CinemaUI.Models;

namespace CinemaUI
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _auth;

        public string? JwtToken { get; private set; }

        public LoginWindow(AuthService auth)
        {
            InitializeComponent();
            _auth = auth;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;  // instance, не статик

            var res = await _auth.LoginAsync(username, password);
            if (res == null)
            {
                MessageBox.Show("Неправильные учётные данные");
                return;
            }

            JwtToken = res.Token;
            DialogResult = true;
        }
    }
}

