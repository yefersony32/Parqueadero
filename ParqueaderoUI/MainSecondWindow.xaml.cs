using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.Viewss;

namespace ParqueaderoUI
{
    public sealed partial class MainSecondWindow : Window
    {
        public MainSecondWindow()
        {
            this.InitializeComponent();
            MainFrame.Navigate(typeof(AdminPage)); 
        }

        private void Ingreso_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(IngresoPage));
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(IngresoPage));
        }
    }
}


