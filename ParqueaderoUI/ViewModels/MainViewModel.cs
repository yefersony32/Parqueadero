
using Microsoft.VisualStudio.PlatformUI;
using ParqueaderoUI.Models;
using ParqueaderoUI.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ParqueaderoUI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Cliente> Clientes { get; set; }

        public MainViewModel()
        {
            _apiService = new ApiService();
            Clientes = new ObservableCollection<Cliente>();
        }

        public async Task CargarClientes()
        {
            var clientes = await _apiService.GetClientesAsync();

            if (clientes.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ No se cargaron clientes en el ViewModel.");
            }

            Clientes.Clear();
            foreach (var cliente in clientes)
            {
                Clientes.Add(cliente);
            }

            System.Diagnostics.Debug.WriteLine($"✅ {Clientes.Count} clientes cargados en la UI.");
        }


    }
}

