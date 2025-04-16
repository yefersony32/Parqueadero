using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.ViewModels;

namespace ParqueaderoUI.Viewss
{
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
            this.Loaded += AdminPage_Loaded;
        }

        private void AdminPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminViewModel viewModel)
            {
                viewModel.TipoReporteSeleccionado = null; // ?? Aseguramos que se inicia sin selección previa
            }
        }

        private async void ReporteSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string seleccion = e.AddedItems[0].ToString();
                if (DataContext is AdminViewModel viewModel)
                {
                    viewModel.TipoReporteSeleccionado = seleccion;
                    await viewModel.ConsultarReporte(); // ?? Se asegura de ejecutar la consulta de forma asíncrona
                }
            }
        }
    }
}

