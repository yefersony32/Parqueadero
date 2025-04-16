using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using ParqueaderoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public ClientesController(ParqueaderoContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteID }, cliente);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.ClienteID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/Clientes/Existe/{cedula}
        [HttpGet("Existe/{cedula}")]
        public async Task<IActionResult> ExisteCliente(string cedula)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula);
            return Ok(cliente != null);
        }

        [HttpGet("BuscarPorNombre/{nombre}")]
        public async Task<ActionResult<object>> GetClientePorNombre(string nombre)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Nombre == nombre);

            if (cliente == null)
            {
                return NotFound();
            }

            // 🔥 Obtener vehículos del cliente
            var vehiculos = await _context.Vehiculos
                .Where(v => v.ClienteID == cliente.ClienteID)
                .Select(v => new { v.Placa, v.Marca, v.Color })
                .ToListAsync();

            // 🔥 Obtener reservas del cliente
            var reservas = await _context.Reservas
                .Where(r => r.Vehiculo.ClienteID == cliente.ClienteID)
                .Select(r => new
                {
                    r.TipoReserva,
                    r.FechaIngreso,
                    r.FechaSalida,
                    Espacio = _context.Espacios.Where(e => e.EspacioID == r.EspacioID).Select(e => e.Nomenclatura).FirstOrDefault()
                })
                .ToListAsync();

            return Ok(new
            {
                cliente.ClienteID,
                cliente.Nombre,
                cliente.Telefono,
                cliente.Correo,
                Vehiculos = vehiculos,
                Reservas = reservas
            });
        }


        [HttpGet("BuscarPorCedula/{cedula}")]
        public async Task<ActionResult<Cliente>> BuscarClientePorCedula(string cedula)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula);

            if (cliente == null)
            {
                return NotFound($"❌ Cliente con cédula {cedula} no encontrado.");
            }

            return Ok(cliente);
        }

    }
}
