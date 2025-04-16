using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.Viewss;

namespace ParqueaderoUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainFrame.Navigate(typeof(IngresoPage)); 
        }

        private void Ingreso_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(IngresoPage));
        }

        private void Salida_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(SalidaPage));
        }
    }
}

