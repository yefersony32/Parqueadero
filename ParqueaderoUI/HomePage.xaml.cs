using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.Services;
using System.Threading.Tasks;

namespace ParqueaderoUI.Viewss
{
    public sealed partial class HomePage : Page
    {
        private readonly ApiService _apiService;

        public HomePage()
        {
            this.InitializeComponent();
            _apiService = new ApiService();
        }

        private void AbrirMain_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Activate();
        }

        private async void ValidarAdmin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = TxtUsuario.Text;
            string contraseña = TxtContraseña.Password;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                TxtError.Text = "⚠️ Usuario y contraseña son obligatorios.";
                TxtError.Visibility = Visibility.Visible;
                return;
            }

            var usuarioValido = await _apiService.LoginAsync(usuario, contraseña);

            if (usuarioValido != null)
            {
                var mainWindow = new MainSecondWindow();
                mainWindow.Activate(); 
            }
            else
            {
                TxtError.Text = "⚠️ Usuario o contraseña incorrectos.";
                TxtError.Visibility = Visibility.Visible;
            }
        }
    }
}
