using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParqueaderoUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ParqueaderoUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://localhost:7073/api/";

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(API_URL) }; 
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(API_URL + "Clientes");
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(response);

                if (clientes == null || clientes.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ No se recibieron clientes de la API.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Se recibieron {clientes.Count} clientes de la API.");
                }

                return clientes;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al obtener clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }
        public async Task<int> ObtenerCantidadArriendosActivosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reservas/ArriendosActivos");

                if (response.IsSuccessStatusCode)
                {
                    var cantidad = await response.Content.ReadAsStringAsync();
                    return int.Parse(cantidad);
                }
                else
                {
                    Debug.WriteLine($"❌ Error en la respuesta: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al obtener arriendos activos: {ex.Message}");
            }

            return 0;
        }

        public async Task<int> RegistrarClienteYObtenerIDAsync(Cliente cliente)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(API_URL + "Clientes", content);

                if (!response.IsSuccessStatusCode)
                {
                    return 0;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var clienteRegistrado = JsonConvert.DeserializeObject<Cliente>(responseBody);
                return clienteRegistrado.ClienteID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar cliente: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> VerificarPlacaExistenteAsync(string placa)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Vehiculos/VerificarPlaca/{placa}");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();
                    return bool.Parse(resultado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al verificar la placa: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> VerificarClientePorPlacaAsync(string placa)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Reservas/VerificarPlaca/{placa}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"⚠️ Respuesta inesperada: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en la verificación de placa: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> ActualizarEstadoEspacioAsync(int espacioID, string nuevoEstado)
        {
            try
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(new { Estado = nuevoEstado }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(API_URL + $"Espacios/{espacioID}", contenido);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado del espacio: {ex.Message}");
                return false;
            }
        }

        public async Task<Espacio> ObtenerEspacioDisponibleAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + "Espacios/Disponible");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Espacio>(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener espacio disponible: {ex.Message}");
                return null;
            }
        }


        public async Task<Vehiculo> RegistrarVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                var json = JsonConvert.SerializeObject(vehiculo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestUrl = new Uri(new Uri(API_URL), "Vehiculos");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"❌ Error al registrar vehículo: {response.StatusCode} - {errorContent}");
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var vehiculoRegistrado = JsonConvert.DeserializeObject<Vehiculo>(jsonResponse);

                Debug.WriteLine($"✅ Vehículo registrado con ID: {vehiculoRegistrado.VehiculoID}");
                return vehiculoRegistrado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Excepción en RegistrarVehiculoAsync: {ex.Message}");
                return null;
            }
        }



        public async Task<bool> VerificarArrendatarioAsync(string placa, string cedula)
        {
            var response = await _httpClient.GetAsync(API_URL + $"Reservas/VerificarArrendatario/{placa}/{cedula}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Cliente> BuscarClientePorCedulaAsync(string cedula)
        {
            var response = await _httpClient.GetAsync(API_URL + $"Clientes/BuscarPorCedula/{cedula}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"📥 Respuesta Cliente: {jsonResponse}");

            return JsonConvert.DeserializeObject<Cliente>(jsonResponse);
        }


        public async Task<Vehiculo> ObtenerVehiculoPorPlacaAsync(string placa)
        {
            var response = await _httpClient.GetAsync(API_URL + $"Vehiculos/ObtenerPorPlaca/{placa}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"📥 Respuesta Vehículo: {jsonResponse}");

            return JsonConvert.DeserializeObject<Vehiculo>(jsonResponse);
        }


        public async Task<bool> VerificarClienteExistenteAsync(string cedula)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Clientes/Existe/{cedula}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return bool.Parse(result);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar cliente: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> VerificarIdentidadAsync(string placa, string cedula)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Reservas/VerificarIdentidad/{placa}/{cedula}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la verificación de identidad: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> CrearReservaAsync(Reserva reserva)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new
                {
                    vehiculoID = reserva.VehiculoID,
                    tipoReserva = reserva.TipoReserva,
                    fechaIngreso = reserva.FechaIngreso,
                    fechaSalida = reserva.FechaSalida,
                    monto = reserva.Monto
                });

                Console.WriteLine($"📤 Enviando JSON a la API:\n{json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(API_URL + "Reservas", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Error al crear reserva: {response.StatusCode} - {errorContent}");
                    return false;
                }

                Console.WriteLine("✅ Reserva creada con éxito desde la UI.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción al crear reserva: {ex.Message}");
                return false;
            }
        }


        public async Task<Espacio> ObtenerEspacioPorPlacaAsync(string placa)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Espacios/PorPlaca/{placa}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Espacio>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener espacio por placa: {ex.Message}");
                return null;
            }
        }


        public async Task<Reserva> ObtenerReservaPorPlacaAsync(string placa)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Reservas/ObtenerPorPlaca/{placa}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ No se encontró reserva para la placa: {placa}");
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Reserva>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener reserva por placa: {ex.Message}");
                return null;
            }
        }

        public async Task<Reserva> ObtenerReservaPorCedulaAsync(string cedula)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + $"Reservas/ObtenerPorCedula/{cedula}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ No se encontró reserva para la cédula: {cedula}");
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Reserva>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener reserva por cédula: {ex.Message}");
                return null;
            }
        }


        public async Task<List<Espacio>> ObtenerEspaciosPorPisoZonaAsync(int piso, string zona)
        {
            try
            {
                var url = $"{API_URL}Espacios/PorPisoZona?piso={piso}&zona={zona}";
                Debug.WriteLine($"[API CALL] Llamando API con URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[API CALL] Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<Espacio>();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[API CALL] Respuesta JSON: {jsonResponse}");

                var espacios = JsonConvert.DeserializeObject<List<Espacio>>(jsonResponse);

                Debug.WriteLine($"[API CALL] API devolvió {espacios.Count} espacios para Piso {piso}, Zona {zona}");

                return espacios;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API CALL] Error al obtener espacios por piso y zona: {ex.Message}");
                return new List<Espacio>();
            }
        }



        // Asignar espacio y marcarlo como ocupado
        public async Task<bool> AsignarEspacioAsync(int espacioID)
        {
            var response = await _httpClient.PostAsync(API_URL + $"Espacios/Asignar/{espacioID}", null);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> GuardarPagoAsync(Pago pago)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(API_URL + "Pagos", pago);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Error en la API: {jsonResponse}");
                    return false;
                }

                Debug.WriteLine($"✅ Respuesta API: {jsonResponse}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en GuardarPagoAsync: {ex.Message}");
                return false;
            }
        }

        private async Task<T> GetFromApiAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL + endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error en la API: {response.StatusCode}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en llamada API {endpoint}: {ex.Message}");
                return default;
            }
        }

        // 📊 1️⃣ Ranking de clientes frecuentes por eventos
        public async Task<List<dynamic>> GetRankingClientesPorEventosAsync()
        {
            return await GetFromApiAsync<List<dynamic>>("Reportes/RankingClientesPorEventos");
        }

        // ⏳ 2️⃣ Ranking de clientes frecuentes por tiempo
        public async Task<List<dynamic>> GetRankingClientesPorTiempoAsync()
        {
            return await GetFromApiAsync<List<dynamic>>("Reportes/RankingClientesPorTiempo");
        }

        // 🚘 3️⃣ Lista de vehículos parqueados en un rango de fechas
        public async Task<List<dynamic>> GetVehiculosParqueadosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            string endpoint = $"Reportes/VehiculosParqueados?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            return await GetFromApiAsync<List<dynamic>>(endpoint);
        }

        // 📅 4️⃣ Lista consolidada de eventos diarios en un mes
        public async Task<List<dynamic>> GetEventosDiariosAsync(int anio, int mes)
        {
            string endpoint = $"Reportes/EventosDiarios?anio={anio}&mes={mes}";
            return await GetFromApiAsync<List<dynamic>>(endpoint);
        }

        // 🏆 5️⃣ Top 15 de espacios más usados
        public async Task<List<dynamic>> GetTopEspaciosAsync()
        {
            string endpoint = "Reportes/TopEspacios";
            return await GetFromApiAsync<List<dynamic>>(endpoint);
        }

        // 💰 6️⃣ Reporte de recaudo diario en un mes (Tipo Tiempo)
        public async Task<List<dynamic>> GetRecaudoDiarioTiempoAsync(int anio, int mes)
        {
            try
            {
                string endpoint = $"Reportes/RecaudoDiarioTiempo?anio={anio}&mes={mes}";
                string urlCompleta = API_URL + endpoint;

                Debug.WriteLine($"📡 Llamando API: {urlCompleta}");

                var response = await _httpClient.GetAsync(urlCompleta);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Error al llamar API: {response.StatusCode}");
                    return new List<dynamic>(); 
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"📡 Respuesta API: {jsonResponse}");

                var recaudos = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
                return recaudos ?? new List<dynamic>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Excepción en GetRecaudoDiarioTiempoAsync: {ex.Message}");
                return new List<dynamic>(); 
            }
        }

        public async Task<List<dynamic>> GetRecaudoDiarioArriendoAsync(int anio, int mes)
        {
            string endpoint = $"Reportes/RecaudoDiarioArriendo?anio={anio}&mes={mes}";
            return await GetFromApiAsync<List<dynamic>>(endpoint);
        }

        public async Task<List<dynamic>> GetArriendosMorososAsync()
        {
            string endpoint = "Reportes/ArriendosMorosos";
            Debug.WriteLine($"📡 Llamando API: {endpoint}");

            var response = await _httpClient.GetAsync(API_URL + endpoint);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"📡 Respuesta API: {jsonResponse}");

            var morosos = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
            return morosos ?? new List<dynamic>();
        }

        public async Task<dynamic> GetClientePorNombreAsync(string nombre)
        {
            return await GetFromApiAsync<dynamic>($"Clientes/BuscarPorNombre/{nombre}");
        }
        public async Task<JObject> GetClientePorPlacaAsync(string placa)
        {
            try
            {
                string endpoint = $"Clientes/BuscarPorPlaca?placa={placa}";
                Debug.WriteLine($"🌍 Llamando API: {endpoint}");

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JObject.Parse(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en GetClientePorPlacaAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> LoginAsync(string nombreUsuario, string contraseña)
        {
            var usuario = new
            {
                NombreUsuario = nombreUsuario,
                ContraseñaHash = contraseña
            };

            var json = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                Debug.WriteLine("🔍 Enviando solicitud de login...");
                var response = await _httpClient.PostAsync("Usuarios/Login", content); 
                Debug.WriteLine($"📡 Código de respuesta: {response.StatusCode}");

                var responseJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"📡 Respuesta JSON: {responseJson}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Debug.WriteLine("❌ Usuario o contraseña incorrectos.");
                        return null;
                    }

                    throw new Exception($"❌ Error en la API: {responseJson}");
                }

                var usuarioDeserializado = JsonConvert.DeserializeObject<Usuario>(responseJson);

                if (usuarioDeserializado == null)
                {
                    throw new Exception("❌ Error al deserializar la respuesta de la API.");
                }

                Debug.WriteLine("✅ Login exitoso.");
                return usuarioDeserializado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Excepción: {ex.Message}");
                throw;
            }
        }



    }
}
