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
    public partial class LoginForm : Window
    {
        private readonly AuthService _auth;
        public string? JwtToken { get; private set; }

        public LoginForm(AuthService authService)
        {
            InitializeComponent();
            _auth = authService;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameBox.Text;
            string pass = PasswordBox.Password;

            var result = await _auth.LoginAsync(login, pass);
            if (result is null)
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            JwtToken = result.Token;
            DialogResult = true;
        }
    }
}

