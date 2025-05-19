using CinemaUI.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CinemaUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализируем API-клиент без токена
            var api = new ApiClient();
            var auth = new AuthService(api);

            // Показываем окно выбора (вход/регистрация)
            var authChoice = new AuthChoiceWindow(auth);
            authChoice.Show();
        }
    }

}
