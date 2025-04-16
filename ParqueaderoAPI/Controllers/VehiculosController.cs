using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using ParqueaderoAPI.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public VehiculosController(ParqueaderoContext context)
        {
            _context = context;
        }

        // GET: api/Vehiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculo()
        {
            return await _context.Vehiculos.ToListAsync();
        }

        // POST: api/Vehiculos
        [HttpPost]
        public async Task<ActionResult<Vehiculo>> RegistrarVehiculo([FromBody] Vehiculo vehiculo)
        {
            try
            {
                if (vehiculo == null || string.IsNullOrEmpty(vehiculo.Placa) || string.IsNullOrEmpty(vehiculo.Tipo) || string.IsNullOrEmpty(vehiculo.Marca) || string.IsNullOrEmpty(vehiculo.Color) || vehiculo.ClienteID == 0)
                {
                    return BadRequest("Datos incompletos para registrar el vehículo.");
                }

                Debug.WriteLine($"Recibiendo vehículo: Placa={vehiculo.Placa}, Tipo={vehiculo.Tipo}, Marca={vehiculo.Marca}, Color={vehiculo.Color}, ClienteID={vehiculo.ClienteID}");

                var clienteExiste = await _context.Clientes.FindAsync(vehiculo.ClienteID);
                if (clienteExiste == null)
                {
                    return BadRequest("El ClienteID no existe en la base de datos.");
                }

                vehiculo.Cliente = null; 
                _context.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(RegistrarVehiculo), new { id = vehiculo.VehiculoID }, vehiculo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al registrar vehículo: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("VerificarPlaca/{placa}")]
        public async Task<IActionResult> VerificarPlaca(string placa)
        {
            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);

            return Ok(vehiculo != null); 
        }

        [HttpGet("ObtenerPorPlaca/{placa}")]
        public async Task<ActionResult<Vehiculo>> ObtenerVehiculoPorPlaca(string placa)
        {
            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);

            if (vehiculo == null)
            {
                return NotFound($"❌ Vehículo con placa {placa} no encontrado.");
            }

            return Ok(vehiculo);
        }


    }
}
    