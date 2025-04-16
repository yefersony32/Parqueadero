using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using ParqueaderoAPI.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public ReservasController(ParqueaderoContext context)
        {
            _context = context;
        }

        // GET: api/Reservas/VerificarPlaca/{placa}
        [HttpGet("VerificarPlaca/{placa}")]
        public async Task<IActionResult> VerificarPlaca(string placa)
        {
            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);
            if (vehiculo == null)
            {
                return NotFound($"❌ Vehículo con placa {placa} no encontrado.");
            }

            return Ok($"✅ Vehículo encontrado: {vehiculo.Placa}");
        }


        // ✅ Método para verificar identidad
        [HttpGet("VerificarIdentidad/{placa}/{documento}")]
        public async Task<IActionResult> VerificarIdentidad(string placa, string documento)
        {
            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);
            if (vehiculo == null)
            {
                return NotFound($"❌ Vehículo con placa {placa} no encontrado.");
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == documento);
            if (cliente == null)
            {
                return NotFound($"❌ Cliente con documento {documento} no encontrado.");
            }

            return Ok($"✅ Identidad verificada para el vehículo {placa} y el cliente {cliente.Nombre}");
        }


        // GET: api/Reservas/ObtenerPorPlaca/{placa}
        [HttpGet("ObtenerPorPlaca/{placa}")]
        public async Task<IActionResult> ObtenerReservaPorPlaca(string placa)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Vehiculo)
                .Include(r => r.Espacio) 
                .FirstOrDefaultAsync(r => r.Vehiculo.Placa == placa && r.FechaSalida == null);

            if (reserva == null)
            {
                return NotFound($"No se encontró reserva activa para la placa: {placa}");
            }

            return Ok(reserva);
        }

        [HttpGet("ObtenerPorCedula/{cedula}")]
        public async Task<IActionResult> ObtenerReservaPorCedula(string cedula)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Vehiculo) 
                .ThenInclude(v => v.Cliente) 
                .Include(r => r.Espacio) 
                .FirstOrDefaultAsync(r => r.Vehiculo.Cliente.Cedula == cedula && r.FechaSalida == null); 

            if (reserva == null)
            {
                return NotFound($"❌ No se encontró reserva activa para la cédula: {cedula}");
            }

            return Ok(reserva);
        }


        [HttpPost]
        public async Task<IActionResult> PostReserva([FromBody] Reserva reserva)
        {
            if (reserva == null)
                return BadRequest("La reserva no puede ser nula.");

            Debug.WriteLine($"📥 Reserva recibida en la API:\n" +
                              $"VehiculoID: {reserva.VehiculoID}\n" +
                              $"TipoReserva: {reserva.TipoReserva}\n" +
                              $"FechaIngreso: {reserva.FechaIngreso}\n" +
                              $"Monto: {reserva.Monto}");

            try
            {
                var vehiculo = await _context.Vehiculos.FindAsync(reserva.VehiculoID);
                if (vehiculo == null)
                    return BadRequest($"❌ Vehículo con ID {reserva.VehiculoID} no encontrado.");

                int arriendosActivos = await _context.Reservas.CountAsync(r => r.TipoReserva == "Reserva" && r.FechaSalida == null);
                if (reserva.TipoReserva == "Reserva" && arriendosActivos >= 25)
                    return BadRequest("⚠️ No se pueden asignar más de 25 arriendos activos.");

                var espacioDisponible = await _context.Espacios
                    .Where(e => e.Estado == "Disponible")
                    .OrderBy(e => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (espacioDisponible == null)
                    return BadRequest("❌ No hay espacios disponibles.");

                reserva.Vehiculo = vehiculo;
                reserva.EspacioID = espacioDisponible.EspacioID;
                reserva.Espacio = espacioDisponible;

                // 🔹 Modificación: Dependiendo del tipo de reserva, cambia el estado
                espacioDisponible.Estado = reserva.TipoReserva == "Tiempo" ? "Ocupado" : "Reservado";
                _context.Espacios.Update(espacioDisponible);

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostReserva), new { id = reserva.ReservaID }, new
                {
                    reserva.ReservaID,
                    reserva.VehiculoID,
                    Espacio = new
                    {
                        espacioDisponible.EspacioID,
                        espacioDisponible.Nomenclatura
                    },
                    reserva.TipoReserva,
                    reserva.FechaIngreso,
                    reserva.FechaSalida,
                    reserva.Monto
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al guardar la reserva: {ex.Message}");
                return StatusCode(500, $"Error al guardar la reserva: {ex.Message}");
            }
        }



        [HttpGet("ArriendosActivos")]
        public async Task<IActionResult> ObtenerCantidadArriendosActivos()
        {
            int cantidad = await _context.Reservas
                .CountAsync(r => r.TipoReserva == "Reserva" && r.FechaSalida == null);

            return Ok(cantidad);
        }


    }
}
