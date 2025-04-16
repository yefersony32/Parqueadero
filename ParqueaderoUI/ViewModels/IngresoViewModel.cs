using CommunityToolkit.Mvvm.ComponentModel;
using ParqueaderoUI.Services;
using ParqueaderoUI.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Diagnostics;
using System.Net.Http;
namespace ParqueaderoUI.ViewModels
{
    public class IngresoViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private string _placa;
        private string _mensaje;
        private bool _mostrarFormularioCliente;
        private bool _mostrarFormularioVehiculo;
        private string _nombre;
        private string _telefono;
        private string _correo;
        private string _cedula;
        private int _clienteID;
        private string _tipoVehiculo;
        private string _marca;
        private string _color;
        private Espacio _espacioAsignado;
        private string _mensajeEspacios;


        private int _pisoSeleccionado;
        private string _zonaSeleccionada;
        private int _vehiculoID; 
        private string _tipoReservaSeleccionado; 
        public ObservableCollection<EspacioParqueo> Espacios { get; set; } = new ObservableCollection<EspacioParqueo>();

        public ObservableCollection<EspacioParqueo> EspaciosColumna1 { get; set; } = new ObservableCollection<EspacioParqueo>();
        public ObservableCollection<EspacioParqueo> EspaciosColumna2 { get; set; } = new ObservableCollection<EspacioParqueo>();

        public ObservableCollection<string> TiposDeReserva { get; set; } = new ObservableCollection<string> { "Tiempo", "Reserva" };

        public ObservableCollection<string> TiposDeVehiculo { get; set; } = new ObservableCollection<string> { "Carro", "Moto" };


        public string TipoReservaSeleccionado
        {
            get => _tipoReservaSeleccionado;
            set => SetProperty(ref _tipoReservaSeleccionado, value);
        }
        public int PisoSeleccionado
        {
            get => _pisoSeleccionado;
            set
            {
                SetProperty(ref _pisoSeleccionado, value);
                Debug.WriteLine($"[ViewModel] Piso seleccionado actualizado: {value}");
            }
        }

        public string ZonaSeleccionada
        {
            get => _zonaSeleccionada;
            set
            {
                SetProperty(ref _zonaSeleccionada, value);
                Debug.WriteLine($"[ViewModel] Zona seleccionada actualizada: {value}");
            }
        }

        public string Placa
        {
            get => _placa;
            set => SetProperty(ref _placa, value);
        }

        public string Mensaje
        {
            get => _mensaje;
            set => SetProperty(ref _mensaje, value);
        }

        public bool MostrarFormularioCliente
        {
            get => _mostrarFormularioCliente;
            set => SetProperty(ref _mostrarFormularioCliente, value);
        }

        public bool MostrarFormularioVehiculo
        {
            get => _mostrarFormularioVehiculo;
            set => SetProperty(ref _mostrarFormularioVehiculo, value);
        }

        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        public string Telefono
        {
            get => _telefono;
            set => SetProperty(ref _telefono, value);
        }

        public string Correo
        {
            get => _correo;
            set => SetProperty(ref _correo, value);
        }

        public string Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }

        public string TipoVehiculo
        {
            get => _tipoVehiculo;
            set { _tipoVehiculo = value; OnPropertyChanged(); }
        }

        public string Marca
        {
            get => _marca;
            set { _marca = value; OnPropertyChanged(); }
        }

        public string Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }

        public Espacio EspacioAsignado
        {
            get => _espacioAsignado;
            set => SetProperty(ref _espacioAsignado, value);
        }

        public string MensajeEspacios
        {
            get => _mensajeEspacios;
            set => SetProperty(ref _mensajeEspacios, value);
        }

        private bool _mostrarFormularioVerificacion;
        public bool MostrarFormularioVerificacion
        {
            get => _mostrarFormularioVerificacion;
            set => SetProperty(ref _mostrarFormularioVerificacion, value);
        }

        public ICommand VerificarPlacaCommand { get; }
        public ICommand RegistrarClienteCommand { get; }
        public ICommand RegistrarVehiculoCommand { get; }
        public ICommand CrearReservaCommand { get; }
        public ICommand AsignarEspacioCommand { get; }
        public ICommand VerificarIdentidadCommand { get; }
        public IRelayCommand CargarEspaciosCommand { get; }

        public IngresoViewModel()
        {
            _apiService = new ApiService();
            VerificarPlacaCommand = new AsyncRelayCommand(VerificarPlaca);
            RegistrarClienteCommand = new AsyncRelayCommand(RegistrarCliente);
            RegistrarVehiculoCommand = new AsyncRelayCommand(RegistrarVehiculo);
            AsignarEspacioCommand = new AsyncRelayCommand(AsignarEspacio);
            VerificarIdentidadCommand = new AsyncRelayCommand(VerificarIdentidad);
            CargarEspaciosCommand = new RelayCommand(CargarEspacios);
            CrearReservaCommand = new AsyncRelayCommand(CrearReserva);
        }

        public async Task CargarEspaciosDirecto(int pisoSeleccionado, string zonaSeleccionada)
        {
            EspaciosColumna1.Clear();
            EspaciosColumna2.Clear();
            MensajeEspacios = "Cargando espacios...";

            Debug.WriteLine($"[CargarEspaciosDirecto] Solicitando espacios para Piso: {pisoSeleccionado}, Zona: {zonaSeleccionada}");

            var espacios = await _apiService.ObtenerEspaciosPorPisoZonaAsync(pisoSeleccionado, zonaSeleccionada);

            if (espacios == null)
            {
                MensajeEspacios = "❌ Error al obtener los espacios. Verifique la conexión.";
                return;
            }

            if (!espacios.Any())
            {
                MensajeEspacios = "⚠️ No hay espacios disponibles para el piso y zona seleccionados.";
                return;
            }

            int mitad = (int)Math.Ceiling(espacios.Count / 2.0);


            int index = 0;
            foreach (var espacio in espacios)
            {
                var espacioParqueo = new EspacioParqueo
                {
                    EspacioID = espacio.EspacioID,
                    Nomenclatura = espacio.Nomenclatura,
                    Piso = espacio.Piso,
                    Zona = espacio.Zona,
                    Estado = espacio.Estado,
                    EstadoColor = espacio.Estado == "Ocupado" ? Brushes.Red :
                                  (espacio.Estado == "Reservado" ? Brushes.Orange : Brushes.Green)
                };

                if (index < mitad)
                {
                    EspaciosColumna1.Add(espacioParqueo);
                }
                else
                {
                    EspaciosColumna2.Add(espacioParqueo);
                }

                index++;
            }

            AsignarPosicionesEspacios();
            MensajeEspacios = ""; 
        }

        public async void CargarEspacios()
        {
            Espacios.Clear();
            MensajeEspacios = "Cargando espacios...";

            Debug.WriteLine($"[CargarEspacios] Piso seleccionado: {PisoSeleccionado}");
            Debug.WriteLine($"[CargarEspacios] Zona seleccionada: {ZonaSeleccionada}");

            var zonaSanitizada = ZonaSeleccionada?.Trim().ToUpper();
            Debug.WriteLine($"[CargarEspacios] Zona sanitizada enviada a la API: {zonaSanitizada}");

            Debug.WriteLine($"[CargarEspacios] Solicitando espacios para Piso: {PisoSeleccionado}, Zona: {zonaSanitizada}");

            var espacios = await _apiService.ObtenerEspaciosPorPisoZonaAsync(PisoSeleccionado, zonaSanitizada);

            if (espacios == null)
            {
                MensajeEspacios = "❌ Error al obtener los espacios. Verifique la conexión.";
                return;
            }

            if (!espacios.Any())
            {
                MensajeEspacios = "⚠️ No hay espacios disponibles para el piso y zona seleccionados.";
                return;
            }

            int index = 0;
            foreach (var espacio in espacios)
            {
                var espacioParqueo = new EspacioParqueo
                {
                    EspacioID = espacio.EspacioID,
                    Nomenclatura = espacio.Nomenclatura,
                    Piso = espacio.Piso,
                    Zona = espacio.Zona,
                    Estado = espacio.Estado,
                    EstadoColor = espacio.Estado == "Ocupado" ? Brushes.Red :
                                  (espacio.Estado == "Reservado" ? Brushes.Orange : Brushes.Green)
                };

                Espacios.Add(espacioParqueo);
                index++;
            }

            AsignarPosicionesEspacios();

            MensajeEspacios = "";  
        }

        private async Task VerificarPlaca()
        {
            var placaExiste = await _apiService.VerificarClientePorPlacaAsync(Placa);

            if (placaExiste)
            {
                Mensaje = "La placa pertenece a un arriendo.\nIngrese su número de documento.";
                MostrarFormularioCliente = true;
                MostrarFormularioVehiculo = false;
                MostrarFormularioVehiculo = false;

                return;
            }

            var espacio = await _apiService.ObtenerEspacioDisponibleAsync();
            if (espacio == null)
            {
                Mensaje = "No hay espacios disponibles.";
                return;
            }

            Mensaje = "Vehículo no registrado. Ingrese los datos.";
            MostrarFormularioCliente = true;
            MostrarFormularioVehiculo = false;
            MostrarFormularioVerificacion = false;

        }

        private async Task VerificarIdentidad()
        {
            var respuesta = await _apiService.VerificarIdentidadAsync(Placa, Cedula);

            if (!respuesta)
            {
                Mensaje = "❌ Identidad no coincide.";
                return;
            }

            var cliente = await _apiService.BuscarClientePorCedulaAsync(Cedula);
            var vehiculo = await _apiService.ObtenerVehiculoPorPlacaAsync(Placa);
            var reservaActual = await _apiService.ObtenerReservaPorPlacaAsync(Placa);

            if (reservaActual != null && reservaActual.TipoReserva == "Arriendo")
            {
                Mensaje = "✅ La placa pertenece a un arriendo. Ingrese su número de documento.";

                MostrarFormularioCliente = false;
                MostrarFormularioVehiculo = false;
                MostrarFormularioVerificacion = false;
                return;
            }

            if (cliente == null && vehiculo == null)
            {
                Mensaje = "⚠️ No se encontró cliente ni vehículo. Registre ambos.";
                MostrarFormularioCliente = true;
                MostrarFormularioVehiculo = true;
                MostrarFormularioVerificacion = false;
                return;
            }

            if (cliente == null)
            {
                Mensaje = "⚠️ Vehículo registrado, pero cliente no encontrado. Registre los datos del cliente.";
                MostrarFormularioCliente = true;
                MostrarFormularioVehiculo = false;
                MostrarFormularioVerificacion = false;
                return;
            }

            _clienteID = cliente.ClienteID;

            if (vehiculo == null)
            {
                Mensaje = "⚠️ Cliente registrado, pero vehículo no encontrado. Registre los datos del vehículo.";
                MostrarFormularioCliente = false;
                MostrarFormularioVehiculo = true;
                MostrarFormularioVerificacion = false;
                return;
            }

            _vehiculoID = vehiculo.VehiculoID;

            if (reservaActual != null && reservaActual.FechaSalida == null)
            {
                Mensaje = $"✅ Identidad verificada. 🚗 Reserva Activa:\n" +
                          $"📍 Espacio: {reservaActual.Espacio?.Nomenclatura ?? "No asignado"}\n" +
                          $"🕒 Ingreso: {reservaActual.FechaIngreso}";

                MostrarFormularioVerificacion = false;
                MostrarFormularioCliente = false;
                MostrarFormularioVehiculo = false;
                return;
            }
            Mensaje = "✅ Identidad verificada. No hay reserva activa. Proceda a realizar una nueva.";
            MostrarFormularioVerificacion = true;
            MostrarFormularioCliente = false;
            MostrarFormularioVehiculo = false;
        }

        private async Task RegistrarCliente()
        {
            var clienteExiste = await _apiService.VerificarClienteExistenteAsync(Cedula);

            if (clienteExiste)
            {
                Mensaje = "El cliente ya está registrado. Proceda a registrar el vehículo.";
                MostrarFormularioVehiculo = true;
                MostrarFormularioCliente = false;
                return;
            }

            var nuevoCliente = new Cliente { Cedula = Cedula, Nombre = Nombre, Telefono = Telefono, Correo = Correo };
            _clienteID = await _apiService.RegistrarClienteYObtenerIDAsync(nuevoCliente);

            if (_clienteID == 0)
            {
                Mensaje = "Error al registrar el cliente.";
                return;
            }

            Mensaje = "Cliente registrado. Ahora registre el vehículo.";
            MostrarFormularioCliente = false;
            MostrarFormularioVehiculo = true;
        }

        private void AsignarPosicionesEspacios()
        {
            int columnas = 4;
            int filas = 3;

            for (int i = 0; i < Espacios.Count; i++)
            {
                Espacios[i].Row = i / columnas;
                Espacios[i].Column = i % columnas;

                Debug.WriteLine($"[AsignarPosiciones] Espacio {Espacios[i].Nomenclatura} -> Row: {Espacios[i].Row}, Column: {Espacios[i].Column}");
            }
        }

        private async Task AsignarEspacio()
        {
            var espacio = await _apiService.ObtenerEspacioDisponibleAsync();
            if (espacio == null)
            {
                MensajeEspacios = "No hay espacios disponibles.";
                return;
            }

            var asignado = await _apiService.AsignarEspacioAsync(espacio.EspacioID);
            if (!asignado)
            {
                Mensaje = "Error al asignar espacio.";
                return;
            }

            EspacioAsignado = espacio;
            Mensaje = $"✅ Espacio asignado: {espacio.Nomenclatura}. 🚦 Talanquera activada.";
        }

        private async Task RegistrarVehiculo()
        {
            if (_clienteID == 0)
            {
                Mensaje = "⚠️ No se ha registrado un cliente válido antes de registrar el vehículo.";
                return;
            }

            bool placaExiste = await _apiService.VerificarPlacaExistenteAsync(Placa);

            if (placaExiste)
            {
                Mensaje = "❌ Esta placa ya está registrada. No se pueden duplicar vehículos.";
                return;
            }

            var nuevoVehiculo = new Vehiculo
            {
                Placa = Placa,
                Tipo = TipoVehiculo,
                Marca = Marca,
                Color = Color,
                ClienteID = _clienteID 
            };

            // Llama a la API y espera la respuesta con el ID
            var vehiculoRegistrado = await _apiService.RegistrarVehiculoAsync(nuevoVehiculo);

            if (vehiculoRegistrado == null)
            {
                Mensaje = "❌ Error al registrar el vehículo.";
                return;
            }

            // ✅ Asignar el VehiculoID recibido
            _vehiculoID = vehiculoRegistrado.VehiculoID;

            Debug.WriteLine($"✅ Vehículo registrado con ID: {_vehiculoID}");

            Mensaje = "✅ Vehículo registrado. Ahora seleccione el tipo de reserva.";
            MostrarFormularioVehiculo = false;
            MostrarFormularioVerificacion = true;
        }


        private async Task CrearReserva()
        {
            if (string.IsNullOrEmpty(TipoReservaSeleccionado))
            {
                Mensaje = "⚠️ Debe seleccionar un tipo de reserva antes de continuar.";
                return;
            }

            if (_vehiculoID == 0)
            {
                Mensaje = "⚠️ No se ha registrado un vehículo válido.";
                return;
            }

            Debug.WriteLine($"📋 VehiculoID usado en la reserva: {_vehiculoID}");

            if (TipoReservaSeleccionado == "Reserva")
            {
                int arriendosActivos = await _apiService.ObtenerCantidadArriendosActivosAsync();
                if (arriendosActivos >= 25)
                {
                    Mensaje = "❌ No se pueden asignar más de 25 espacios para arriendo.";
                    return;
                }
            }

            var nuevaReserva = new Reserva
            {
                VehiculoID = _vehiculoID,
                TipoReserva = TipoReservaSeleccionado,
                FechaIngreso = DateTime.Now,
                FechaSalida = null,
                Monto = CalcularMonto(TipoReservaSeleccionado)
            };

            Debug.WriteLine($"📤 Datos de la reserva a enviar:\n" +
                            $"VehiculoID: {nuevaReserva.VehiculoID}\n" +
                            $"TipoReserva: {nuevaReserva.TipoReserva}\n" +
                            $"FechaIngreso: {nuevaReserva.FechaIngreso}\n" +
                            $"Monto: {nuevaReserva.Monto}");

            bool reservaCreada = await _apiService.CrearReservaAsync(nuevaReserva);

            if (!reservaCreada)
            {
                Mensaje = "❌ Error al crear la reserva desde la UI.";
                return;
            }

            var reservaConEspacio = await _apiService.ObtenerReservaPorPlacaAsync(Placa);

            if (reservaConEspacio != null && reservaConEspacio.Espacio != null)
            {
                Mensaje = $"✅ Reserva creada con éxito.\n" +
                          $"🚗 Vehículo ID: {reservaConEspacio.VehiculoID}\n" +
                          $"📋 Tipo de Reserva: {reservaConEspacio.TipoReserva}\n" +
                          $"📅 Fecha Ingreso: {reservaConEspacio.FechaIngreso}\n" +
                          $"💲 Monto Inicial: {reservaConEspacio.Monto:C}\n" +
                          $"🚦 Espacio asignado: {reservaConEspacio.Espacio.Nomenclatura}\n";
            }
            else
            {
                Mensaje = $"✅ Reserva creada, pero no se pudo obtener el espacio asignado.";
            }
        }

        private decimal CalcularMonto(string tipoReserva)
        {
            decimal monto = 0;

            if (tipoReserva == "Tiempo")
            {
                decimal tarifaBase = 1000;
                monto = tarifaBase;
            }
            else if (tipoReserva == "Reserva")
            {
                decimal tarifaBaseArriendo = 200000;
                monto = tarifaBaseArriendo;
            }

            return monto;
        }

    }
    public class EspacioParqueo : Espacio
    {
        public Brush EstadoColor { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }





}