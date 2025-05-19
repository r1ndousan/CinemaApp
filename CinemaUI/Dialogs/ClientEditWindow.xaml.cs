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


namespace CinemaUI
{
    public partial class ClientEditWindow : Window
    {
        public ClientDto Client { get; }

        public ClientEditWindow(ClientDto existing)
        {
            InitializeComponent();
            Client = existing;
            NameBox.Text = existing.Name;
            LoginBox.Text = existing.Login;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Client.Name = NameBox.Text;
            Client.Login = LoginBox.Text;
            Client.PasswordHash = PassBox.Password;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

