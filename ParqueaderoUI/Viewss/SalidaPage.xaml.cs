using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.ViewModels;

namespace ParqueaderoUI.Viewss
{
    public sealed partial class SalidaPage : Page
    {
        public SalidaPage()
        {
            this.InitializeComponent();
            this.DataContext = new SalidaViewModel(); // ?? Asegura que el ViewModel se cargue correctamente
        }
    }
}

