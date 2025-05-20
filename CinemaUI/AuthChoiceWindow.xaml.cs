using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CinemaUI.Services;

namespace CinemaUI
{
    public partial class AuthChoiceWindow : Window
    {
        private readonly AuthService _authService;

        public AuthChoiceWindow(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWin = new LoginForm(_authService);
            if (loginWin.ShowDialog() == true)
            {
                // Получаем токен из LoginForm
                string jwt = loginWin.JwtToken!;
                // Открываем основное окно
                var mainWin = new MainWindow(jwt);
                mainWin.Show();
                this.Close();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var regWin = new RegisterForm(_authService);
            regWin.ShowDialog();
        }
    }
}

