using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using ParqueaderoUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace ParqueaderoUI.ViewModels
{
    public class AdminViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public AdminViewModel()

        {
            Debug.WriteLine($"🔍 Reporte Seleccionado: {TipoReporteSeleccionado}");

            _apiService = new ApiService();
            ConsultarCommand = new AsyncRelayCommand(ConsultarReporte);

            TiposDeReportes = new ObservableCollection<string>
             {
             "Ranking Clientes por Eventos",
             "Ranking Clientes por Tiempo",
             "Vehículos Parqueados (Tipo Tiempo)",
             "Eventos Diarios",
              "Top 15 Espacios Más Usados",
              "Recaudo Diario (Tiempo)",  
              "Recaudo Diario (Arriendo)", 
               "Lista de Arriendos Morosos" 

            };


            AñosDisponibles = new ObservableCollection<int>(Enumerable.Range(DateTime.Now.Year - 10, 11));
            MesesDisponibles = new ObservableCollection<string>(new[]
            {
                 "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                 "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            });
        }

        public ObservableCollection<string> TiposDeReportes { get; }
        public ObservableCollection<int> AñosDisponibles { get; }
        public ObservableCollection<string> MesesDisponibles { get; }

        private int _anioSeleccionado = DateTime.Now.Year;
        public int AnioSeleccionado
        {
            get => _anioSeleccionado;
            set => SetProperty(ref _anioSeleccionado, value);
        }

        private string _mesSeleccionado = "Enero"; 
        public string MesSeleccionado
        {
            get => _mesSeleccionado;
            set
            {
                if (SetProperty(ref _mesSeleccionado, value) && MesesDict.ContainsKey(value))
                {
                    NumeroMesSeleccionado = MesesDict[value]; 
                }
            }
        }
        private int _numeroMesSeleccionado = 1;
        public int NumeroMesSeleccionado
        {
            get => _numeroMesSeleccionado;
            set => SetProperty(ref _numeroMesSeleccionado, value);
        }

        private string _tipoReporteSeleccionado;
        public string TipoReporteSeleccionado
        {
            get => _tipoReporteSeleccionado;
            set
            {
                if (SetProperty(ref _tipoReporteSeleccionado, value))
                {
                    Debug.WriteLine($"🔍 Reporte Seleccionado: {value}");

                    MostrarFiltroFecha = value == "Vehículos Parqueados (Tipo Tiempo)";
                    MostrarFiltroAnioMes = value == "Eventos Diarios" ||
                                           value == "Recaudo Diario (Tiempo)" ||
                                           value == "Recaudo Diario (Arriendo)";

                    ReporteDatos.Clear();
                    ReporteDatosVisible = false;

                    if (value == "Ranking Clientes por Eventos" ||
                        value == "Ranking Clientes por Tiempo" ||
                        value == "Top 15 Espacios Más Usados" ||
                        value == "Lista de Arriendos Morosos")
                    {
                        _ = ConsultarReporte();
                    }
                    else
                    {
                        ReporteDatos.Clear();
                        ReporteDatosVisible = false; 
                    }

                    OnPropertyChanged(nameof(MostrarFiltroFecha));
                    OnPropertyChanged(nameof(MostrarFiltroAnioMes));
                }
            }
        }

        private static readonly Dictionary<string, int> MesesDict = new()
        {
            { "Enero", 1 }, { "Febrero", 2 }, { "Marzo", 3 }, { "Abril", 4 },
            { "Mayo", 5 }, { "Junio", 6 }, { "Julio", 7 }, { "Agosto", 8 },
            { "Septiembre", 9 }, { "Octubre", 10 }, { "Noviembre", 11 }, { "Diciembre", 12 }
        };

        private bool _mostrarFiltroFecha;
        public bool MostrarFiltroFecha
        {
            get => _mostrarFiltroFecha;
            set => SetProperty(ref _mostrarFiltroFecha, value);
        }

        private bool _reporteDatosVisible;
        public bool ReporteDatosVisible
        {
            get => _reporteDatosVisible;
            set => SetProperty(ref _reporteDatosVisible, value);
        }

        private bool _mostrarFiltroAnioMes;
        public bool MostrarFiltroAnioMes
        {
            get => _mostrarFiltroAnioMes;
            set => SetProperty(ref _mostrarFiltroAnioMes, value);
        }

        private string _reporteSeleccionado;
        public string ReporteSeleccionado
        {
            get => _reporteSeleccionado;
            set
            {
                if (SetProperty(ref _reporteSeleccionado, value))
                {
                    LimpiarDetalles(); 
                    Debug.WriteLine($"📊 Reporte seleccionado: {value}");

                    GenerarReporteDetalle();
                }
            }
        }
        private string _detalleReporte;
        public string DetalleReporte
        {
            get => _detalleReporte;
            set
            {
                SetProperty(ref _detalleReporte, value);
                Debug.WriteLine($"🔄 Actualizando DetalleReporte: {value}");

                OnPropertyChanged(nameof(MostrarDetalleReporte)); 
                OnPropertyChanged(nameof(MostrarDetallesCliente)); 
            }
        }

        private void GenerarReporteDetalle()
        {
            if (string.IsNullOrEmpty(ReporteSeleccionado))
            {
                DetalleReporte = "";
                return;
            }

            Debug.WriteLine($"📊 Generando Reporte Detallado: {ReporteSeleccionado}");

            DetalleCliente = "";
            OnPropertyChanged(nameof(DetalleCliente));
            OnPropertyChanged(nameof(MostrarDetallesCliente));

            DetalleReporte = $"📌 Resumen del Día:\n{ReporteSeleccionado}";
            OnPropertyChanged(nameof(DetalleReporte));
            OnPropertyChanged(nameof(MostrarDetalleReporte));
        }

        private ObservableCollection<string> _reporteDatos = new();
        public ObservableCollection<string> ReporteDatos
        {
            get => _reporteDatos;
            set => SetProperty(ref _reporteDatos, value);
        }

        public ICommand ConsultarCommand { get; }

        private DateTime _fechaInicio = DateTime.Now.AddDays(-7);
        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set => SetProperty(ref _fechaInicio, value);
        }

        private DateTime _fechaFin = DateTime.Now;
        public DateTime FechaFin
        {
            get => _fechaFin;
            set => SetProperty(ref _fechaFin, value);
        }

        public async Task ConsultarReporte()
        {
            try
            {
                if (string.IsNullOrEmpty(TipoReporteSeleccionado)) return;

                ReporteDatos.Clear();
                ReporteDatosVisible = false; 

                if (TipoReporteSeleccionado == "Ranking Clientes por Eventos")
                {
                    var rankingEventos = await _apiService.GetRankingClientesPorEventosAsync();
                    foreach (var item in rankingEventos)
                    {
                        string nombreCliente = item.nombreCliente ?? "Desconocido";
                        int numeroEventos = item.numeroEventos;
                        ReporteDatos.Add($"{nombreCliente} - {numeroEventos} eventos");
                    }
                    ReporteDatosVisible = true;

                }
                else if (TipoReporteSeleccionado == "Ranking Clientes por Tiempo")
                {
                    var rankingTiempo = await _apiService.GetRankingClientesPorTiempoAsync();
                    foreach (var item in rankingTiempo)
                    {
                        string nombreCliente = item.nombreCliente ?? "Desconocido";
                        int tiempoTotal = item.tiempoTotal;
                        ReporteDatos.Add($"{nombreCliente} - {tiempoTotal} minutos");
                    }
                    ReporteDatosVisible = true;

                }
                else if (TipoReporteSeleccionado == "Eventos Diarios")
                {
                    var eventosDiarios = await _apiService.GetEventosDiariosAsync(AnioSeleccionado, NumeroMesSeleccionado);
                    foreach (var item in eventosDiarios)
                    {
                        string fecha = DateTime.Parse(item.fecha.ToString()).ToString("dd/MM/yyyy");
                        int numeroEventos = item.numeroEventos;
                        ReporteDatos.Add($"📅 {fecha}: {numeroEventos} eventos");
                    }
                    ReporteDatosVisible = true;
                }

                else if (TipoReporteSeleccionado == "Vehículos Parqueados (Tipo Tiempo)")
                {
                    var vehiculos = await _apiService.GetVehiculosParqueadosAsync(FechaInicio, FechaFin);
                    foreach (var item in vehiculos)
                    {
                        string placa = item.placa ?? "Desconocida";
                        string marca = item.marca ?? "Sin marca";
                        string color = item.color ?? "Sin color";
                        string fechaIngreso = DateTime.Parse(item.fechaIngreso.ToString()).ToString("dd/MM/yyyy HH:mm");
                        string fechaSalida = item.fechaSalida != null
                            ? DateTime.Parse(item.fechaSalida.ToString()).ToString("dd/MM/yyyy HH:mm")
                            : "Aún en parqueadero";

                        ReporteDatos.Add($"🚗 {placa} - {marca} ({color})\n🕒 Ingreso: {fechaIngreso} - Salida: {fechaSalida}");
                    }
                    ReporteDatosVisible = true;
                }
                else if (TipoReporteSeleccionado == "Top 15 Espacios Más Usados")
                {
                    Debug.WriteLine($"📡 Consultando Top 15 Espacios Más Usados...");

                    var espacios = await _apiService.GetTopEspaciosAsync();

                    Debug.WriteLine($"📡 Respuesta API: {Newtonsoft.Json.JsonConvert.SerializeObject(espacios)}");

                    if (espacios == null || !espacios.Any())
                    {
                        ReporteDatos.Add("✅ No hay información de espacios usados.");
                        ReporteDatosVisible = true;
                        return;
                    }

                    ReporteDatos.Clear();

                    foreach (var item in espacios)
                    {
                        string espacioID = item["espacioID"].ToString();
                        string nomenclatura = item["nomenclatura"].ToString();
                        int vecesUsado = Convert.ToInt32(item["vecesUsado"]);

                        ReporteDatos.Add($"🅿️ Espacio: {nomenclatura} (ID: {espacioID})\n🔄 Veces Usado: {vecesUsado}");
                    }

                    ReporteDatosVisible = true;
                    OnPropertyChanged(nameof(ReporteDatos)); 
                }



                else if (TipoReporteSeleccionado == "Recaudo Diario (Tiempo)")
                {
                    var recaudos = await _apiService.GetRecaudoDiarioTiempoAsync(AnioSeleccionado, NumeroMesSeleccionado);

                    if (recaudos == null || !recaudos.Any())
                    {
                        ReporteDatos.Add("⚠️ No hay datos de recaudo para este mes.");
                        ReporteDatosVisible = true;
                        return;
                    }

                    foreach (var item in recaudos)
                    {
                        try
                        {
                            string fechaTexto = item["fecha"].ToString();
                            string fechaFormateada = DateTime.Parse(fechaTexto).ToString("dd/MM/yyyy");

                            decimal totalRecaudado = Convert.ToDecimal(item["totalRecaudado"]);

                            ReporteDatos.Add($"📅 {fechaFormateada}: 💰 {totalRecaudado:C}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"❌ Error al procesar recaudo: {ex.Message}");
                            ReporteDatos.Add("❌ Error al mostrar datos.");
                        }
                    }

                    ReporteDatosVisible = true;
                }

                else if (TipoReporteSeleccionado == "Recaudo Diario (Arriendo)")
                {
                    var recaudos = await _apiService.GetRecaudoDiarioArriendoAsync(AnioSeleccionado, NumeroMesSeleccionado);

                    if (recaudos == null || !recaudos.Any())
                    {
                        ReporteDatos.Add("⚠️ No hay datos de recaudo por arriendo para este mes.");
                        ReporteDatosVisible = true;
                        return;
                    }

                    foreach (var item in recaudos)
                    {
                        string fechaTexto = item["fecha"].ToString();
                        string fechaFormateada = DateTime.Parse(fechaTexto).ToString("dd/MM/yyyy");
                        decimal totalRecaudado = Convert.ToDecimal(item["totalRecaudado"]);

                        ReporteDatos.Add($"📅 {fechaFormateada}: 💰 {totalRecaudado:C}");
                    }
                    ReporteDatosVisible = true;
                }


                else if (TipoReporteSeleccionado == "Lista de Arriendos Morosos")
                {
                    Debug.WriteLine($"📡 Consultando Lista de Arriendos Morosos...");

                    var morosos = await _apiService.GetArriendosMorososAsync();

                    Debug.WriteLine($"📡 Respuesta API: {Newtonsoft.Json.JsonConvert.SerializeObject(morosos)}");

                    if (morosos == null || !morosos.Any())
                    {
                        ReporteDatos.Add("✅ No hay arriendos morosos.");
                        ReporteDatosVisible = true;
                        return;
                    }

                    ReporteDatos.Clear();

                    foreach (var item in morosos)
                    {
                        string cliente = item["cliente"]?.ToString() ?? "Desconocido";
                        string placa = item["placa"]?.ToString() ?? "Sin placa";
                        string espacio = item["espacio"]?.ToString() ?? "Sin espacio";
                        string fechaUltimoPago = string.IsNullOrEmpty(item["fechaUltimoPago"]?.ToString()) ? "Sin registro" : item["fechaUltimoPago"].ToString();
                        int diasMora = Convert.ToInt32(item["diasMora"]);
                        string metodoPago = item["metodoPago"]?.ToString() ?? "No registrado";
                        decimal montoAdeudado = Convert.ToDecimal(item["montoAdeudado"]);

                        ReporteDatos.Add($"👤 Cliente: {cliente}\n🚗 Placa: {placa}\n🅿️ Espacio: {espacio}\n📅 Último Pago: {fechaUltimoPago}\n⏳ Días en Mora: {diasMora}\n💳 Método Pago: {metodoPago}\n💰 Monto Adeudado: {montoAdeudado:C}");
                    }

                    ReporteDatosVisible = true;
                    OnPropertyChanged(nameof(ReporteDatos));
                }


                ReporteDatosVisible = true; 
            }
            catch (Exception ex)
            {
                ReporteDatos.Add($"❌ Error: {ex.Message}");
            }
        }

        private string _clienteSeleccionado;
        public string ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                if (SetProperty(ref _clienteSeleccionado, value))
                {
                    Debug.WriteLine($"🟢 Cliente seleccionado: {value}");

                    if (TipoReporteSeleccionado == "Eventos Diarios" || TipoReporteSeleccionado == "Vehículos Parqueados (Tipo Tiempo)")
                    {
                        Debug.WriteLine($"⚠️ No se busca cliente en el reporte '{TipoReporteSeleccionado}'");
                        return;
                    }
                    DetalleReporte = ""; 
                    OnPropertyChanged(nameof(DetalleReporte));
                    OnPropertyChanged(nameof(MostrarDetalleReporte));
                    _ = CargarDetallesCliente();
                }
            }
        }

        private string _detalleCliente;
        public string DetalleCliente
        {
            get => _detalleCliente;
            set
            {
                SetProperty(ref _detalleCliente, value);
                OnPropertyChanged(nameof(MostrarDetallesCliente)); 
                OnPropertyChanged(nameof(MostrarDetalleReporte)); 
            }
        }

        public bool MostrarDetallesCliente => !string.IsNullOrEmpty(DetalleCliente) && string.IsNullOrEmpty(DetalleReporte);
        public bool MostrarDetalleReporte
        {
            get => !string.IsNullOrEmpty(DetalleReporte);
        }
        public bool MostrarDetallesClienteYReporte => string.IsNullOrEmpty(DetalleReporte);
        private void LimpiarDetalles()
        {
            DetalleReporte = ""; 
            DetalleCliente = ""; 
            OnPropertyChanged(nameof(MostrarDetallesCliente));
            OnPropertyChanged(nameof(MostrarDetalleReporte));
        }
        private async Task CargarDetallesCliente()
        {
            try
            {
                if (string.IsNullOrEmpty(ClienteSeleccionado))
                {
                    Debug.WriteLine("⚠️ No hay cliente seleccionado.");
                    return;
                }

                string identificador = ClienteSeleccionado.Split('-')[0].Trim();
                JObject clienteData = null;

                if (TipoReporteSeleccionado == "Vehículos Parqueados (Tipo Tiempo)")
                {
                    Debug.WriteLine($"🔍 Buscando cliente por placa: {identificador}");
                    clienteData = await _apiService.GetClientePorPlacaAsync(identificador);
                }
                else if (TipoReporteSeleccionado == "Eventos Diarios")
                {
                    Debug.WriteLine($"⚠️ Reporte '{TipoReporteSeleccionado}' no tiene clientes, omitiendo búsqueda.");
                    return;
                }
                else
                {
                    Debug.WriteLine($"🔍 Buscando cliente por nombre: {identificador}");
                    clienteData = await _apiService.GetClientePorNombreAsync(identificador);
                }

                if (clienteData != null)
                {
                    string nombreCliente = clienteData["nombre"]?.ToString() ?? "Desconocido";
                    string telefono = clienteData["telefono"]?.ToString() ?? "No registrado";
                    string correo = clienteData["correo"]?.ToString() ?? "No registrado";

                    var vehiculosArray = clienteData["vehiculos"] as JArray ?? new JArray();
                    string vehiculosTexto = vehiculosArray.Any()
                        ? string.Join("\n", vehiculosArray.Select(v => $"🚗 {v["placa"]} - {v["marca"]} ({v["color"]})"))
                        : "Ninguno";

                    var reservasArray = clienteData["reservas"] as JArray ?? new JArray();
                    string reservasTexto = reservasArray.Any()
                        ? string.Join("\n", reservasArray.Select(r =>
                        {
                            string tipoReserva = r["tipoReserva"]?.ToString() ?? "Desconocido";
                            string fechaIngreso = DateTime.Parse(r["fechaIngreso"].ToString()).ToString("dd/MM/yyyy HH:mm");
                            string fechaSalida = r["fechaSalida"] != null && !string.IsNullOrEmpty(r["fechaSalida"].ToString())
                                ? DateTime.Parse(r["fechaSalida"].ToString()).ToString("dd/MM/yyyy HH:mm")
                                : "Aún en parqueadero";
                            string espacio = r["espacio"]?.ToString() ?? "No asignado";

                            return $"🅿️ {tipoReserva} - Ingreso: {fechaIngreso} - Salida: {fechaSalida} - Espacio: {espacio}";
                        }))
                        : "❌ No tiene reservas registradas.";

                    DetalleCliente = $"👤 Nombre: {nombreCliente}\n📞 Teléfono: {telefono}\n📧 Correo: {correo}\n\n🚘 **Vehículos:**\n{vehiculosTexto}\n\n📌 **Reservas:**\n{reservasTexto}";

                    Debug.WriteLine($"✅ Cliente encontrado: {DetalleCliente}");

                    OnPropertyChanged(nameof(DetalleCliente));
                    OnPropertyChanged(nameof(MostrarDetallesCliente));
                }
                else
                {
                    Debug.WriteLine("❌ No se encontró información del cliente.");
                    DetalleCliente = "❌ Cliente no encontrado.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al cargar detalles del cliente: {ex.Message}");
                DetalleCliente = $"❌ Error: {ex.Message}";
            }
        }

    }
}

