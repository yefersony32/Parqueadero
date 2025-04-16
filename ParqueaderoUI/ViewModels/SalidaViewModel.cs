using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParqueaderoUI.Models;
using ParqueaderoUI.Services;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParqueaderoUI.ViewModels
{
    public class SalidaViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        private string _placaODocumento;
        public string PlacaODocumento
        {
            get => _placaODocumento;
            set => SetProperty(ref _placaODocumento, value);
        }

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        private Reserva _reservaEncontrada;
        public Reserva ReservaEncontrada
        {
            get => _reservaEncontrada;
            set => SetProperty(ref _reservaEncontrada, value);
        }

        private decimal _montoTotal;
        public decimal MontoTotal
        {
            get => _montoTotal;
            set => SetProperty(ref _montoTotal, value);
        }

        // Pago
        private string _metodoPagoSeleccionado;
        public string MetodoPagoSeleccionado
        {
            get => _metodoPagoSeleccionado;
            set
            {
                SetProperty(ref _metodoPagoSeleccionado, value);
                OnPropertyChanged(nameof(EsPagoPSE));
                OnPropertyChanged(nameof(EsPagoTarjeta));
                OnPropertyChanged(nameof(EsPagoEfectivo));
            }
        }   

        // Propiedades para PSE
        public string EntidadFinanciera { get; set; }
        public string TitularCuenta { get; set; }
        public string NumeroCuenta { get; set; }

        // Propiedades para Tarjeta
        public string TitularTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public string FechaVencimiento { get; set; }
        public string CodigoCVV { get; set; }

        private string _montoEfectivoTexto;
        public string MontoEfectivoTexto
        {
            get => _montoEfectivoTexto;
            set
            {
                if (decimal.TryParse(value, out decimal monto))
                {
                    MontoEfectivo = monto;  
                    Debug.WriteLine($"💵 Monto ingresado en efectivo: {MontoEfectivo}");
                }
                SetProperty(ref _montoEfectivoTexto, value);
            }
        }

        private decimal _montoEfectivo;
        public decimal MontoEfectivo
        {
            get => _montoEfectivo;
            set => SetProperty(ref _montoEfectivo, value);
        }

        private string _mensajeCambio;
        public string MensajeCambio
        {
            get => _mensajeCambio;
            set => SetProperty(ref _mensajeCambio, value);
        }
        public bool EsPagoPSE => MetodoPagoSeleccionado == "PSE";
        public bool EsPagoTarjeta => MetodoPagoSeleccionado == "Tarjeta de Crédito";
        public bool EsPagoEfectivo => MetodoPagoSeleccionado == "Efectivo";

        public ICommand BuscarVehiculoCommand { get; }
        public ICommand RealizarPagoCommand { get; }

        public SalidaViewModel()
        {
            _apiService = new ApiService();
            BuscarVehiculoCommand = new AsyncRelayCommand(BuscarVehiculo);
            RealizarPagoCommand = new AsyncRelayCommand(RealizarPago);
        }

        private async Task BuscarVehiculo()
        {
            if (string.IsNullOrWhiteSpace(PlacaODocumento))
            {
                Mensaje = "⚠️ Ingrese una placa o documento válido.";
                return;
            }

            ReservaEncontrada = await _apiService.ObtenerReservaPorPlacaAsync(PlacaODocumento);

            if (ReservaEncontrada == null)
            {
                ReservaEncontrada = await _apiService.ObtenerReservaPorCedulaAsync(PlacaODocumento);
            }

            if (ReservaEncontrada == null)
            {
                Mensaje = "❌ No se encontró reserva activa.";
                return;
            }

            MontoTotal = CalcularMonto(ReservaEncontrada);
            Mensaje = $"✅ Reserva encontrada. Total a pagar: {MontoTotal:C}";
        }


        private decimal CalcularMonto(Reserva reserva)
        {
            if (reserva.TipoReserva == "Reserva")
                return 200000; 

            TimeSpan tiempo = DateTime.Now - reserva.FechaIngreso;
            int horas = (int)Math.Ceiling(tiempo.TotalHours);
            return horas * 3000; 
        }

        private async Task RealizarPago()
        {
            Debug.WriteLine($"📌 MontoEfectivo en RealizarPago: {MontoEfectivo}");

            if (ReservaEncontrada == null)
            {
                Mensaje = "❌ No hay una reserva asociada al pago.";
                return;
            }

            if (MontoTotal <= 0)
            {
                Mensaje = "⚠️ No hay un monto válido a pagar.";
                return;
            }

            if (string.IsNullOrEmpty(MetodoPagoSeleccionado))
            {
                Mensaje = "⚠️ Seleccione un método de pago.";
                return;
            }

            if (EsPagoTarjeta && !Regex.IsMatch(FechaVencimiento, "^(0[1-9]|1[0-2])/[0-9]{2}$"))
            {
                Mensaje = "⚠️ Formato de fecha de vencimiento incorrecto. Use MM/AA.";
                return;
            }

            if (EsPagoEfectivo && MontoEfectivo < MontoTotal)
            {
                Mensaje = "❌ Monto insuficiente para el pago en efectivo.";
                return;
            }

            var nuevoPago = new Pago
            {
                ReservaID = ReservaEncontrada.ReservaID,
                MetodoPago = MetodoPagoSeleccionado,
                Monto = EsPagoEfectivo ? MontoEfectivo : MontoTotal, 
                FechaPago = DateTime.Now
            };

            bool pagoGuardado = await _apiService.GuardarPagoAsync(nuevoPago);

            if (pagoGuardado)
            {
                Mensaje = "✅ Pago exitoso y registrado. Espacio liberado y reserva cerrada.";
                if (EsPagoEfectivo)
                {
                    MensajeCambio = $"💵 Cambio: {(MontoEfectivo - MontoTotal):C}";
                }
            }
            else
            {
                Mensaje = "⚠️ El pago fue procesado, pero hubo un error al guardarlo en la base de datos.";
            }
        }


    }
}
