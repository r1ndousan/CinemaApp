using CinemaUI.Services;
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

namespace CinemaUI
{
    public partial class RegisterForm : Window
    {
        private readonly AuthService _auth;

        public RegisterForm(AuthService authService)
        {
            InitializeComponent();
            _auth = authService;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameBox.Text;
            string pass = PasswordBox.Password;
            string conf = ConfirmBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (pass != conf)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Вызываем регистрацию
            var resp = await _auth.RegisterAsync(login, pass);
            if (!resp.IsSuccessStatusCode)
            {
                MessageBox.Show($"Не удалось зарегистрироваться ({resp.StatusCode}).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Регистрация прошла успешно. Теперь войдите.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
