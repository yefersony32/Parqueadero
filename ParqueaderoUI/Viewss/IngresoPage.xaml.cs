using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ParqueaderoUI.ViewModels;
using System.Diagnostics;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ParqueaderoUI.Viewss
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IngresoPage : Page
    {
        public IngresoPage()
        {
            InitializeComponent();
            this.DataContext = new IngresoViewModel();
        }
        private async void CargarEspacios_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el ViewModel
            var viewModel = DataContext as IngresoViewModel;

            // Leer el valor seleccionado del ComboBox de Piso
            var pisoItem = PisoComboBox.SelectedItem as ComboBoxItem;
            int pisoSeleccionado = int.Parse(pisoItem.Content.ToString());

            // Leer el valor seleccionado del ComboBox de Zona
            var zonaItem = ZonaComboBox.SelectedItem as ComboBoxItem;
            string zonaSeleccionada = zonaItem.Content.ToString();

            // Mostrar en consola para depurar
            Debug.WriteLine($"[CargarEspacios] Piso seleccionado: {pisoSeleccionado}");
            Debug.WriteLine($"[CargarEspacios] Zona seleccionada: {zonaSeleccionada}");

            // Llama al método del ViewModel pasando los valores seleccionados
            await viewModel.CargarEspaciosDirecto(pisoSeleccionado, zonaSeleccionada);
        }


    }

}
