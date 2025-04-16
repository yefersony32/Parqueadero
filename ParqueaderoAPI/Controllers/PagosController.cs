using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using ParqueaderoAPI.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public PagosController(ParqueaderoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostPago([FromBody] Pago pago)
        {
            if (pago == null)
            {
                Debug.WriteLine("❌ El objeto Pago está vacío.");
                return BadRequest("El objeto Pago está vacío.");
            }

            Debug.WriteLine($"📥 Recibido Pago para ReservaID: {pago.ReservaID}, Monto: {pago.Monto}, MetodoPago: {pago.MetodoPago}");

            var reserva = await _context.Reservas
                .Include(r => r.Espacio)
                .FirstOrDefaultAsync(r => r.ReservaID == pago.ReservaID);

            if (reserva == null)
            {
                Debug.WriteLine($"❌ No se encontró la reserva con ID {pago.ReservaID}.");
                return NotFound($"No se encontró la reserva con ID {pago.ReservaID}.");
            }

            if (pago.Monto <= 0)
            {
                Debug.WriteLine("❌ El monto del pago es inválido.");
                return BadRequest("El monto del pago debe ser mayor a 0.");
            }

            try
            {
                pago.FechaPago = DateTime.Now;
                _context.Pagos.Add(pago);
                Debug.WriteLine($"💾 Pago agregado a la base de datos con FechaPago={pago.FechaPago}");

                reserva.FechaSalida = pago.FechaPago;
                _context.Entry(reserva).State = EntityState.Modified;

                if (reserva.Espacio != null)
                {
                    reserva.Espacio.Estado = "Disponible";
                    _context.Entry(reserva.Espacio).State = EntityState.Modified;
                }

                var cambios = await _context.SaveChangesAsync();
                Debug.WriteLine($"✅ Cambios guardados en la base de datos. Total de entidades afectadas: {cambios}");

                return CreatedAtAction(nameof(PostPago), new { id = pago.PagoID }, pago);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al guardar el pago: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, $"Error al guardar el pago: {ex.InnerException?.Message ?? ex.Message}");
            }


        }
    }
}
